using Funday.ServiceInterface.StockxApi;
using Funday.ServiceModel.Inventory;
using Funday.ServiceModel.StockXAccount;
using Microsoft.AspNetCore.Hosting;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using StockxApi;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Funday.ServiceInterface
{
    public class InventoryService : Service
    {
        private readonly IHostingEnvironment _env;
        private readonly IAppSettings _settings;
        private readonly IAuthRepository _authRepository;
        private static ILog Log = LogManager.GetLogger(typeof(InventoryService));

        public InventoryService(IAppSettings settings, IAuthRepository authRepository, IHostingEnvironment env)
        {
            _env = env;
            _settings = settings;
            _authRepository = authRepository;
        }

        private class ValidateListInventory : AbstractValidator<ListInventoryRequest>
        {
            public ValidateListInventory()
            {
                RuleFor(A => A.Skip).GreaterThan(-1);
            }
        }

        [Authenticate]
        // [RequiresAnyRole("")]
        public ListInventoryResponse Get(ListInventoryRequest request)
        {
            var Validation = new ValidateListInventory();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new ListInventoryResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }

            try
            {
                var Inventorys = Db.SelectMulti<Inventory, StockXAccount>(Db.From<Inventory>().Join<StockXAccount>((A, B) => A.StockXAccountId == B.Id).OrderBy(A => A.Id).Skip(request.Skip).Take(50));
                var CountOf = Db.Count<Inventory>();
                return new ListInventoryResponse()
                {
                    Success = true,
                    Total = CountOf,
                    Inventorys = Inventorys
                };
            }
            catch (Exception ex)
            {
                return new ListInventoryResponse()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        private class ValidateUpdateInventory : AbstractValidator<UpdateInventoryRequest>
        {
            public ValidateUpdateInventory()
            {
                RuleFor(x => x.Quantity).GreaterThan(-1);
                RuleFor(x => x.StartingAsk).GreaterThan(-1);

                RuleFor(x => x.MinSell).GreaterThan(-1);
            }
        }

        [Authenticate]
        // [RequiresAnyRole("")]
        public UpdateInventoryResponse Put(UpdateInventoryRequest request)
        {
            var Validation = new ValidateUpdateInventory();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new UpdateInventoryResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
            var User = this.GetCurrentAppUser();
            var ExistingInventory = Db.Single<Inventory>(A => A.Id == request.InventoryId && A.UserId == User.Id);
            if (ExistingInventory == null)
            {
                return new UpdateInventoryResponse()
                {
                    Success = false,
                    Message = "No Such Inventory"
                };
            }
 
            var TotalUpdated = Db.UpdateOnly(() => new Inventory() { StartingAsk = request.StartingAsk, MinSell = request.MinSell, Quantity = request.Quantity }, A => A.Id == ExistingInventory.Id);

          

            return new UpdateInventoryResponse()
            {
                TotalUpdated = TotalUpdated,
                Success = true,
            };
        }

        private class ValidateCreateInventory : AbstractValidator<CreateInventoryRequest>
        {
            public ValidateCreateInventory()
            {
                RuleFor(x => x.StockXUrl).NotEmpty().Matches(new Regex("^https://stockx.com/")).WithMessage("Must be Stockx Link");
                RuleFor(x => x.Size).NotEmpty();
            }
        }

        [Authenticate]
        // [RequiresAnyRole("")]
        public async Task<CreateInventoryResponse> Post(CreateInventoryRequest request)
        {
            var Validation = new ValidateCreateInventory();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new CreateInventoryResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
            var User = this.GetCurrentAppUser();
                
            var ExistingStockXAccount = Db.Single<StockXAccount>(A => A.Id == request.StockXAccountId && A.UserId == User.Id);
            if (ExistingStockXAccount == null)
            {
                return new CreateInventoryResponse()
                {
                    Success = false,
                    Message = "No Such StockXAccount"
                };
            }
            request.StockXUrl = request.StockXUrl.ToLower();
            var NewInventory = new Inventory()
            {
                StockXUrl = request.StockXUrl,
                Quantity = request.Quantity,
                MinSell = request.MinSell,
                UserId=User.Id,
                StartingAsk = request.StartingAsk,
                Size = request.Size,
            };
            try
            {
                var Existing = Db.Single(Db.From<StockXProxuct>().Where(A => A.Url == request.StockXUrl && A.Description == request.Size ));
                if (Existing != null)
                {
                    NewInventory.Active = true;
                    NewInventory.StockXAccountId = request.StockXAccountId;
                    NewInventory.ParentSku = Existing.ParentSku;
                    NewInventory.Sku = Existing.Sku;
                }
                else
                {
                    StockXAccount tmp = new StockXAccount()
                    {
                        
                    };
                    var List = await tmp.GetProductFromUrl(request.StockXUrl);
                    if (List == null)
                    {
                        return new CreateInventoryResponse()
                        {
                            Success = false,
                            Message = "Could not get product data"
                        };
                    }
                    if (List.Offers.OffersOffers.Length == 0)
                    {
                        return new CreateInventoryResponse()
                        {
                            Success = false,
                            Message = "Could not get any product data"
                        };
                    }

                    if (request.Size == "-1")
                    {
                        var Item = List.Offers.OffersOffers[0];
                        NewInventory.Sku = Item.Sku;
                        NewInventory.Size = "-1";
                    }
                    else
                    {
                        var Sized = List.Offers.OffersOffers.Where(A => A.Description == request.Size).FirstOrDefault();
                        if (Sized == null)
                        {
                            return new CreateInventoryResponse()
                            {
                                Success = false,
                                Message = "Could not get any product data with that size"
                            };
                        }
                        NewInventory.Active = true;
                        NewInventory.StockXAccountId = request.StockXAccountId;
                        NewInventory.ParentSku = List.Sku;
                        NewInventory.Sku = Sized.Sku;
                        
                    }

                    try
                    {
                        foreach (var Offer in List.Offers.OffersOffers)
                        {
                            if (!Db.Exists<StockXProxuct>(A => A.Sku == NewInventory.Sku))
                            {
                                Offer.Url = NewInventory.StockXUrl;
                                Offer.ParentSku = NewInventory.ParentSku;
                                Db.Insert(Offer);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new CreateInventoryResponse()
                        {
                            Success = false,
                            Message = ex.Message
                        };
                    }
                }
                try
                {
                    if (Db.Exists<Inventory>(A => A.Sku == NewInventory.Sku && A.StockXAccountId == request.StockXAccountId))
                    {
                        return new CreateInventoryResponse()
                        {
                            Success = false,
                            Message="This Inventory Exists for this Account, swap it to another account by removing quantity"

                        };
                    }
                    var InsertedId = Db.Insert(NewInventory, true);
                    return new CreateInventoryResponse()
                    {
                        InsertedId = InsertedId,
                        Success = true,
                    };
                }
                catch (Exception ex)
                {
                    return new CreateInventoryResponse()
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
            catch (Exception ex)
            {
                return new CreateInventoryResponse()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [Authenticate]
        // [RequiresAnyRole("")]
        public ToggleInventoryResponse Post(ToggleInventoryRequest request)
        {
            var User = this.GetCurrentAppUser();
            var ExistingInventory = Db.Single<Inventory>(A => A.Id == request.InventoryId && A.UserId == User.Id);
            if (ExistingInventory == null)
            {
                return new ToggleInventoryResponse()
                {
                    Success = false,
                    Message = "No Such Inventory"
                };
            }
            ExistingInventory.Active = !ExistingInventory.Active;
            Db.Update(ExistingInventory);
            return new ToggleInventoryResponse()
            {
                Success = true,
            };
        }

        private class ValidateDeleteInventory : AbstractValidator<DeleteInventoryRequest>
        {
            public ValidateDeleteInventory()
            {
            }
        }

        [Authenticate]
        // [RequiresAnyRole("")]
        public DeleteInventoryResponse Delete(DeleteInventoryRequest request)
        {
            var Validation = new ValidateDeleteInventory();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new DeleteInventoryResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
            var User = this.GetCurrentAppUser();
            var ExistingInventory = Db.Single<Inventory>(A => A.Id == request.InventoryId && A.UserId == User.Id);
            if (ExistingInventory == null)
            {
                return new DeleteInventoryResponse()
                {
                    Success = false,
                    Message = "No Such Inventory"
                };
            }

            var Deleted = Db.DeleteById<Inventory>(request.InventoryId);

            return new DeleteInventoryResponse()
            {
                TotalDeleted = Deleted,
                Success = true,
            };
        }

        [Authenticate]
        // [RequiresAnyRole("")]
        public ListOneInventoryResponse Get(ListOneInventoryRequest request)
        {
            var User = this.GetCurrentAppUser();
            var ExistingInventory = Db.Single<Inventory>(A => A.Id == request.InventoryId && A.UserId == User.Id);
            if (ExistingInventory == null)
            {
                return new ListOneInventoryResponse()
                {
                    Success = false,
                    Message = "No Such Inventory"
                };
            }
            return new ListOneInventoryResponse()
            {
                InventoryItem = ExistingInventory,
                Success = true,
            };
        }
    }
}