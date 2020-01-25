using Funday.ServiceModel.DiscordNotifications;
using Funday.ServiceModel.StockXAccount;
using Funday.ServiceModel.StockXListedItem;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;

namespace Funday.ServiceInterface
{
    public static class StockxListingEvent
    {
        public static DiscordNotifications GetWebHooks(this StockXAccount Login)
        {
            using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
            {
                return Db.Single<DiscordNotifications>(A => A.UserId == Login.UserId && !A.Disabled);
            }
        }

        public static void IncrementError(this DiscordNotifications Disco)
        {
            using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
            {
                Db.UpdateAdd(() => new DiscordNotifications() { Errors = 1 }, A => A.Id == Disco.Id);
            }
        }

        public static void ResetErrors(this DiscordNotifications Disco)
        {
            using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
            {
                Db.UpdateOnly(() => new DiscordNotifications() { Errors = 0 }, A => A.Id == Disco.Id);
            }
        }
        public static void Disable(this DiscordNotifications Disco)
        {
            using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
            {
                Db.UpdateOnly(() => new DiscordNotifications() { Disabled = true }, A => A.Id == Disco.Id);
            }
        }
        
        public static async Task Sold(StockXListedItem I, StockXAccount Login)
        {
            DiscordNotifications WebHook = null;
            try
            {
                WebHook = Login.GetWebHooks();
                if (WebHook == null || WebHook.Sold == null) return;

                Discord.Webhook.DiscordWebhookClient discordWebhookClient = new Discord.Webhook.DiscordWebhookClient(WebHook.Sold);
                var Success = await discordWebhookClient.SendMessageAsync($"{$"{Login.Email} has sold a "}{I.Product.Shoe} ({I.Product.ShoeSize}) at {I.LocalCurrency}{I.LocalAmount}");
                if (WebHook.Errors > 0)
                {
                    WebHook.ResetErrors();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (WebHook != null)
                    {
                        WebHook.IncrementError();
                        if(WebHook.Errors > 3) //so at 5
                        {
                            WebHook.Disable();
                        }
                    }
                }
                catch (Exception ex2)
                {
                }
            }
        }
        public static async Task Listed(StockXListedItem I, StockXAccount Login)
        {
            DiscordNotifications WebHook = null;
            try
            {
                WebHook = Login.GetWebHooks();
                if (WebHook == null || WebHook.Listing == null) return;

                Discord.Webhook.DiscordWebhookClient discordWebhookClient = new Discord.Webhook.DiscordWebhookClient(WebHook.Listing);
                var Success = await discordWebhookClient.SendMessageAsync($"{$"{Login.Email} has listed a "}{I.Product.Shoe} ({I.Product.ShoeSize}) at {I.LocalCurrency}{I.LocalAmount}");
                if (WebHook.Errors > 0)
                {
                    WebHook.ResetErrors();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (WebHook != null)
                    {
                        WebHook.IncrementError();
                        if (WebHook.Errors > 3) //so at 5
                        {
                            WebHook.Disable();
                        }
                    }
                }
                catch (Exception ex2)
                {
                }
            }
        }
        public static async Task Disabled(StockXAccount Login)
        {
            DiscordNotifications WebHook = null;
            try
            {
                WebHook = Login.GetWebHooks();
                if (WebHook == null || WebHook.Listing == null) return;

                Discord.Webhook.DiscordWebhookClient discordWebhookClient = new Discord.Webhook.DiscordWebhookClient(WebHook.Error);
                var Success = await discordWebhookClient.SendMessageAsync($"{Login.Email} has failed too many logins and is disabled");
                if (WebHook.Errors > 0)
                {
                    WebHook.ResetErrors();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (WebHook != null)
                    {
                        WebHook.IncrementError();
                        if (WebHook.Errors > 3) //so at 5
                        {
                            WebHook.Disable();
                        }
                    }
                }
                catch (Exception ex2)
                {
                }
            }
        }

   
    }
}