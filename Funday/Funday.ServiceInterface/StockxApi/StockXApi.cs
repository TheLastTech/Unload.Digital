using Funday.ServiceModel;
using Funday.ServiceModel.Inventory;
using Funday.ServiceModel.StockXAccount;
using Newtonsoft.Json;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using StockxApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Funday.ServiceInterface.StockxApi
{
    public class StockXApiResult<T>
    {
        public HttpStatusCode Code { get; set; }
        public string ResultText { get; set; }
        public T RO { get; set; }
    }

    public static class StockXApi
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static async Task<StockXApiResult<LoginCookieToken>> GetLogin(this StockXAccount stockAuth)
        {
            var tmpdr = Path.GetTempFileName();
            var Json = Base64Encode(JsonConvert.SerializeObject(stockAuth));
            ProcessStartInfo Processstartinfo;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Processstartinfo = new ProcessStartInfo("/usr/bin/xvfb-run", $" /usr/bin/node --require ts-node/register StockLoginator.ts --BaseJson={Json} --JobID={tmpdr}   ")
                {
                    UseShellExecute = false,

                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + "NodeStuff"
                };
            }
            else
            {
                string arguments = $"--require ts-node/register StockLoginator.ts --BaseJson={Json} --JobID={tmpdr} ";
                Processstartinfo = new ProcessStartInfo("node", arguments)
                {
                    UseShellExecute = true,

                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "NodeStuff"
                };
            }

            string jsontxt = "";
            string outputtxt = "";
            try
            {
                var Proc = new Process
                {
                    StartInfo = Processstartinfo
                };
                Proc.Start();

                //Error = Proc.StandardError.ReadToEnd();
                //   Output = Proc.StandardOutput.ReadToEnd();
                var MaxWait = 40;
                while (!Proc.WaitForExit(5000) && MaxWait-- > 0)
                {
                    await Task.Delay(500);
                }
                if (MaxWait == 0)
                {
                    try
                    {
                        Proc.Kill();
                        AuditExtensions.CreateAudit(stockAuth.Id, "StockXApi", "GetLogin", "Killed Process");
                    }
                    catch (Exception ex)
                    {
                        AuditExtensions.CreateAudit(stockAuth.Id, "StockXApi", "GetLogin", "Error Killing Process",ex.Message,ex.StackTrace);
                    }
                }
                if (File.Exists(tmpdr + ".txt"))
                {
                    outputtxt = File.ReadAllText(tmpdr + ".txt");
                    File.Delete(tmpdr + ".txt");
                }
                if (File.Exists(tmpdr))
                {
                    jsontxt = File.ReadAllText(tmpdr);
                    File.Delete(tmpdr);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var JsonObj = JsonConvert.DeserializeObject<LoginCookieToken>(jsontxt, settings);

                    if (JsonObj != null && JsonObj.error == null)
                        return new StockXApiResult<LoginCookieToken>()
                        {
                            Code = HttpStatusCode.OK,
                            ResultText = jsontxt + outputtxt,
                            RO = JsonObj
                        };
                }
                else
                {
                    using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
                    {
                        AuditExtensions.CreateAudit(Db, stockAuth.Id, "FunBoy/GetLogin", "Login Attempt", "Login Failed: " + jsontxt + outputtxt);
                    }
                }
                return new StockXApiResult<LoginCookieToken>()
                {
                    Code = HttpStatusCode.Ambiguous,
                    ResultText = jsontxt + outputtxt + tmpdr,
                };
            }
            catch (Exception ex)
            {
                using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
                {
                    AuditExtensions.CreateAudit(Db, stockAuth.Id, "FunBoy/GetLogin", "Login Attempt", "Login Failed: " + jsontxt + outputtxt, ex.Message, ex.StackTrace);
                }
                return new StockXApiResult<LoginCookieToken>()
                {
                    Code = HttpStatusCode.InternalServerError,
                    ResultText = AppDomain.CurrentDomain.BaseDirectory + "NodeStuff" + ex.Message + ex.StackTrace + outputtxt + jsontxt + tmpdr,
                };
            }
        }

        public static async Task<StockXApiResult<GetPagedPortfolioItemsResponse>> GetCurrentPending(this StockXAccount stockAuth, string CusomterID, int Page = 1)
        {
            using (var httpClient = stockAuth.GetHttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://stockx.com/api/customers/{CusomterID}/selling/pending?sort=created_at&order=DESC&limit=20&page={Page}&currency={stockAuth.Currency}&country={stockAuth.Country}"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "stockx.com");
                    request.Headers.TryAddWithoutValidation("dnt", "1");
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + stockAuth.Token);
                    request.Headers.TryAddWithoutValidation("appos", "web");
                    request.Headers.TryAddWithoutValidation("x-requested-with", "XMLHttpRequest");
                    request.Headers.TryAddWithoutValidation("x-anonymous-id", "undefined");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("appversion", "0.1");
                    request.Headers.TryAddWithoutValidation("accept", "*/*");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("referer", "https://stockx.com/selling");
                    request.Headers.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");

                    var response = await httpClient.SendAsync(request);
                    var txt = await response.Content.ReadAsStringAsync();
                    if ((int)response.StatusCode > 399)
                    {
                        return new StockXApiResult<GetPagedPortfolioItemsResponse>()
                        {
                            Code = response.StatusCode,
                            ResultText = txt,
                        };
                    }
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return new StockXApiResult<GetPagedPortfolioItemsResponse>()
                    {
                        Code = response.StatusCode,
                        ResultText = txt,
                        RO = JsonConvert.DeserializeObject<GetPagedPortfolioItemsResponse>(txt, settings)
                    };
                }
            }
        }

        public static async Task<List<PortfolioItem>> GetAllPending(this StockXAccount login)
        {
            List<PortfolioItem> OutputItems = new List<PortfolioItem>();
            var GetListings = await login.GetCurrentPending(login.CustomerID.ToString(), 1);

            if ((int)GetListings.Code > 399 && (int)GetListings.Code < 500)
            {
                throw new NeedsVerificaitonException(login);
            }
            GetPagedPortfolioItemsResponse Items = GetListings.RO;

            if (Items.PortfolioItems == null || Items.PortfolioItems.Count == 0)
            {
                return null;
            }
            OutputItems.AddRange(Items.PortfolioItems);
            var i = 2;
            while (Items.Pagination != null && Items.Pagination.NextPage != null)
            {
                GetListings = await login.GetCurrentPending(login.CustomerID.ToString(), i++);

                if ((int)GetListings.Code > 399 && (int)GetListings.Code < 500)
                {
                    throw new NeedsVerificaitonException(login);
                }
                Items = GetListings.RO;
                if (Items.PortfolioItems == null || Items.PortfolioItems.Count == 0)
                {
                    return null;
                }
                OutputItems.AddRange(Items.PortfolioItems);
            }
            return OutputItems;
        }

        public static async Task<List<PortfolioItem>> GetAllListings(this StockXAccount login)
        {
            List<PortfolioItem> OutputItems = new List<PortfolioItem>();
            StockXApiResult<GetPagedPortfolioItemsResponse> GetListings;
            try
            {
                GetListings = await login.GetCurrentListings(login.CustomerID.ToString(), 1);
            }
            catch (Exception ex)
            {
                AuditExtensions.CreateAudit(login.Id, "StockXApi", "GetAllListings", "Error", ex.Message, ex.StackTrace);
                return null;
            }
            if ((int)GetListings.Code > 399 && (int)GetListings.Code < 500)
            {
                throw new NeedsVerificaitonException(login);
            }
            GetPagedPortfolioItemsResponse Items = GetListings.RO;

            if (Items.PortfolioItems == null || Items.PortfolioItems.Count == 0)
            {
                return null;
            }
            OutputItems.AddRange(Items.PortfolioItems);
            var i = 2;
            while (Items.Pagination != null && Items.Pagination.NextPage != null)
            {
                GetListings = await login.GetCurrentListings(login.CustomerID.ToString(), i++);

                if ((int)GetListings.Code > 399 && (int)GetListings.Code < 500)
                {
                    throw new NeedsVerificaitonException(login);
                }
                Items = GetListings.RO;
                if (Items.PortfolioItems == null || Items.PortfolioItems.Count == 0)
                {
                    break;
                }
                OutputItems.AddRange(Items.PortfolioItems);
            }
            return OutputItems;
        }

        public static HttpClient GetHttpClient(this StockXAccount stockAuth)
        {
            WebProxy Proxy = null;
            if (!string.IsNullOrEmpty(stockAuth.ProxyHost))
            {
                Proxy = new WebProxy($"http://{stockAuth.ProxyHost}:{stockAuth.ProxyPort}");

                if (stockAuth.ProxyUsername != null && stockAuth.ProxyPassword != null)
                {
                    Proxy.Credentials = new NetworkCredential(stockAuth.ProxyUsername, stockAuth.ProxyPassword);
                }
            }

            HttpClientHandler proxyhandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                UseCookies = true,
                //Proxy = Proxy,
                AllowAutoRedirect = true,
                UseProxy = false,
            };

            return new HttpClient(proxyhandler);
        }

        public static async Task<ProductList> GetProductFromUrl(this StockXAccount stockAuth, string Url)
        {
            using (var httpClient = stockAuth.GetHttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), Url))
                {
                    request.Headers.TryAddWithoutValidation("authority", "stockx.com");
                    request.Headers.TryAddWithoutValidation("dnt", "1");
                    request.Headers.TryAddWithoutValidation("upgrade-insecure-requests", "1");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "none");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "navigate");
                    request.Headers.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");

                    var response = await httpClient.SendAsync(request);
                    var txt = await response.Content.ReadAsStringAsync();
                    var StartProduct = txt.IndexOf(@"""@type"":""Product""");
                    if (StartProduct >= 0)
                    {
                        var EndProduct = txt.IndexOf("</script", StartProduct);
                        if (EndProduct > StartProduct)
                        {
                            var settings = new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                MissingMemberHandling = MissingMemberHandling.Ignore
                            };
                            var data = txt.Substring(StartProduct, EndProduct - StartProduct);

                            return JsonConvert.DeserializeObject<ProductList>("{" + data, settings);
                        }
                    }
                }
                return null;
            }
        }

        public static async Task<StockXApiResult<GetPagedPortfolioItemsResponse>> GetCurrentListings(this StockXAccount stockAuth, string CusomterID, int Page, string IfNoneMatch = "")
        {
            using (var httpClient = stockAuth.GetHttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://stockx.com/api/customers/{CusomterID}/selling/current?sort=created_at&order=DESC&limit=20&page={Page}&currency={stockAuth.Currency}&country={stockAuth.Country}"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "stockx.com");
                    request.Headers.TryAddWithoutValidation("dnt", "1");
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + stockAuth.Token);
                    request.Headers.TryAddWithoutValidation("appos", "web");
                    request.Headers.TryAddWithoutValidation("x-requested-with", "XMLHttpRequest");
                    request.Headers.TryAddWithoutValidation("x-anonymous-id", "undefined");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("appversion", "0.1");
                    request.Headers.TryAddWithoutValidation("accept", "*/*");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("referer", "https://stockx.com/selling");
                    request.Headers.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");

                    var response = await httpClient.SendAsync(request);
                    var txt = await response.Content.ReadAsStringAsync();

                    if ((int)response.StatusCode > 399)
                    {
                        return new StockXApiResult<GetPagedPortfolioItemsResponse>()
                        {
                            Code = response.StatusCode,
                            ResultText = txt,
                        };
                    }

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    return new StockXApiResult<GetPagedPortfolioItemsResponse>()
                    {
                        Code = response.StatusCode,
                        ResultText = txt,
                        RO = JsonConvert.DeserializeObject<GetPagedPortfolioItemsResponse>(txt, settings)
                    };
                }
            }
        }

        public static async Task<StockXApiResult<object>> DeleteListin(this StockXAccount stockAuth, string ChainID)
        {
            using (var httpClient = stockAuth.GetHttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("DELETE"), "https://stockx.com/api/portfolio/" + ChainID))
                {
                    request.Headers.TryAddWithoutValidation("authority", "stockx.com");
                    request.Headers.TryAddWithoutValidation("origin", "https://stockx.com");
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + stockAuth.Token);
                    request.Headers.TryAddWithoutValidation("content-type", "application/json");
                    request.Headers.TryAddWithoutValidation("appos", "web");
                    request.Headers.TryAddWithoutValidation("x-requested-with", "XMLHttpRequest");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("dnt", "1");
                    request.Headers.TryAddWithoutValidation("appversion", "0.1");
                    request.Headers.TryAddWithoutValidation("accept", "*/*");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("referer", "https://stockx.com/sell/" + ChainID);
                    request.Headers.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");
                    var json = new
                    {
                        chain_id = "",
                        notes = "User Canceled Bid"
                    };
                    request.Content = new StringContent(JsonConvert.SerializeObject(json));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                    var txt = await response.Content.ReadAsStringAsync();
                    if ((int)response.StatusCode > 399)
                    {
                        return new StockXApiResult<object>()
                        {
                            Code = response.StatusCode,
                            ResultText = txt,
                        };
                    }
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return new StockXApiResult<object>()
                    {
                        Code = response.StatusCode,
                        ResultText = txt,
                        RO = JsonConvert.DeserializeObject<GetPagedPortfolioItemsResponse>(txt, settings)
                    };
                }
            }
        }

        public static async Task<StockXApiResult<ProductActivityResponse>> GetBids(this StockXAccount stockAuth, Inventory Item, int Page = 1)
        {
            using (var httpClient = stockAuth.GetHttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://stockx.com/api/products/{Item.Sku}/activity?state=300&currency={stockAuth.Currency}&limit=100&page={Page}&sort=amount&order=DESC&country={stockAuth.Country}"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "stockx.com");
                    request.Headers.TryAddWithoutValidation("dnt", "1");
                    request.Headers.TryAddWithoutValidation("appos", "web");
                    request.Headers.TryAddWithoutValidation("x-requested-with", "XMLHttpRequest");
                    request.Headers.TryAddWithoutValidation("x-anonymous-id", "undefined");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");

                    request.Headers.TryAddWithoutValidation("appversion", "0.1");
                    request.Headers.TryAddWithoutValidation("accept", "*/*");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("referer", Item.StockXUrl);
                    request.Headers.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");

                    var response = await httpClient.SendAsync(request);
                    var txt = await response.Content.ReadAsStringAsync();
                    if ((int)response.StatusCode > 399)
                    {
                        return new StockXApiResult<ProductActivityResponse>()
                        {
                            Code = response.StatusCode,
                            ResultText = txt,
                        };
                    }
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return new StockXApiResult<ProductActivityResponse>()
                    {
                        Code = response.StatusCode,
                        ResultText = txt,
                        RO = JsonConvert.DeserializeObject<ProductActivityResponse>(txt, settings)
                    };
                }
            }
        }

        public static async Task<StockXApiResult<ProductActivityResponse>> GetASks(this StockXAccount stockAuth, Inventory Item, int Page = 1)
        {
            using (var httpClient = stockAuth.GetHttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://stockx.com/api/products/{Item.Sku}/activity?state=400&currency={stockAuth.Currency}&limit=100&page={Page}&sort=amount&order=ASC&country={stockAuth.Country}"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "stockx.com");
                    request.Headers.TryAddWithoutValidation("dnt", "1");

                    request.Headers.TryAddWithoutValidation("appos", "web");
                    request.Headers.TryAddWithoutValidation("x-requested-with", "XMLHttpRequest");
                    request.Headers.TryAddWithoutValidation("x-anonymous-id", "undefined");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("appversion", "0.1");
                    request.Headers.TryAddWithoutValidation("accept", "*/*");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("referer", Item.StockXUrl);
                    request.Headers.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");

                    var response = await httpClient.SendAsync(request);
                    var txt = await response.Content.ReadAsStringAsync();
                    if ((int)response.StatusCode > 399)
                    {
                        return new StockXApiResult<ProductActivityResponse>()
                        {
                            Code = response.StatusCode,
                            ResultText = txt,
                        };
                    }
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return new StockXApiResult<ProductActivityResponse>()
                    {
                        Code = response.StatusCode,
                        ResultText = txt,
                        RO = JsonConvert.DeserializeObject<ProductActivityResponse>(txt, settings)
                    };
                }
            }
        }

        public static async Task<StockXApiResult<object>> UpdateListing(this StockXAccount stockAuth, string ChainID, string skuUuid, string ExpiresDate, int Amount)
        {
            using (var httpClient = stockAuth.GetHttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://stockx.com/api/portfolio?a=ask"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "stockx.com");
                    request.Headers.TryAddWithoutValidation("origin", "https://stockx.com");
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + stockAuth.Token);
                    request.Headers.TryAddWithoutValidation("content-type", "application/json");
                    request.Headers.TryAddWithoutValidation("appos", "web");
                    request.Headers.TryAddWithoutValidation("x-requested-with", "XMLHttpRequest");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("dnt", "1");
                    request.Headers.TryAddWithoutValidation("appversion", "0.1");
                    request.Headers.TryAddWithoutValidation("accept", "*/*");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("referer", "https://stockx.com/sell/" + ChainID);
                    request.Headers.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");
                    var NewListing = new StockXUpdateAskRequest()
                    {
                        Action = "ask",
                        PortfolioItem = new StockXUpdateAskPortfolioItem()
                        {
                            LocalAmount = Amount,
                            ExpiresAt = ExpiresDate,
                            SkuUuid = skuUuid,
                            LocalCurrency = stockAuth.Currency,
                            Meta = new StockXUpdateAskMeta()
                            {
                                DiscountCode = "",
                            },
                            ChainId = ChainID,
                        }
                    };
                    request.Content = new StringContent(JsonConvert.SerializeObject(NewListing));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await httpClient.SendAsync(request);
                    var txt = await response.Content.ReadAsStringAsync();
                    if ((int)response.StatusCode > 399)
                    {
                        return new StockXApiResult<object>()
                        {
                            Code = response.StatusCode,
                            ResultText = txt,
                        };
                    }
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return new StockXApiResult<object>()
                    {
                        Code = response.StatusCode,
                        ResultText = txt,
                        RO = JsonConvert.DeserializeObject<ProductActivityResponse>(txt, settings)
                    };
                }
            }
        }

        public static async Task<StockXApiResult<ListItemResponse>> MakeListing(this StockXAccount stockAuth, string RefUrl, string skuUuid, int Amount, string ExpiresDate)
        {
            using (var httpClient = stockAuth.GetHttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://stockx.com/api/portfolio?a=ask"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "stockx.com");
                    request.Headers.TryAddWithoutValidation("origin", "https://stockx.com");
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + stockAuth.Token);
                    request.Headers.TryAddWithoutValidation("content-type", "application/json");
                    request.Headers.TryAddWithoutValidation("appos", "web");
                    request.Headers.TryAddWithoutValidation("x-requested-with", "XMLHttpRequest");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("dnt", "1");
                    request.Headers.TryAddWithoutValidation("appversion", "0.1");
                    request.Headers.TryAddWithoutValidation("accept", "*/*");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("referer", RefUrl.Replace("//stockx.com/", "//stockx.com/sell/"));

                    request.Headers.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");
                    var NewListing = new ListItemRequest()
                    {
                        Action = "ask",
                        PortfolioItem = new ListItemRequestPortfolioItem()
                        {
                            LocalAmount = Amount,
                            ExpiresAt = ExpiresDate,
                            SkuUuid = skuUuid,
                            LocalCurrency = stockAuth.Currency,
                            Meta = new ListItemRequestMeta
                            {
                                DiscountCode = "",
                            }
                        }
                    };
                    var PostJson = JsonConvert.SerializeObject(NewListing);
                    request.Content = new StringContent(PostJson);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await httpClient.SendAsync(request);
                    var txt = await response.Content.ReadAsStringAsync();

                    if ((int)response.StatusCode > 399)
                    {
                        return new StockXApiResult<ListItemResponse>()
                        {
                            Code = response.StatusCode,
                            ResultText = txt
                        };
                    }

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return new StockXApiResult<ListItemResponse>()
                    {
                        RO = JsonConvert.DeserializeObject<ListItemResponse>(txt, settings),
                        Code = response.StatusCode,
                        ResultText = txt
                    };
                }
            }
        }
    }
}