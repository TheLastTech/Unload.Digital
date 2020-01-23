using Microsoft.AspNetCore.Hosting;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Logging; 
using System.Linq;
using Funday.ServiceModel;
using ServiceStack.OrmLite;
using ServiceStack.FluentValidation;
using Funday.ServiceModel.StockXListedItem;
using Funday.ServiceModel.Inventory;
using Funday.ServiceModel.StockXAccount;

namespace Funday.ServiceInterface
{
    public class StockXListedItemService : Service
    {
        private readonly IHostingEnvironment _env;
        private readonly IAppSettings _settings;
        private readonly IAuthRepository _authRepository;
        private static ILog Log = LogManager.GetLogger(typeof(StockXListedItemService));
      

        public StockXListedItemService(IAppSettings settings, IAuthRepository authRepository, IHostingEnvironment env )
        {
           
            _env = env;
            _settings = settings;
            _authRepository = authRepository;
        }
        
        private class ValidateListStockXListedItem : AbstractValidator<ListStockXListedItemRequest>
        {
            public ValidateListStockXListedItem()
            {
                RuleFor(A => A.Skip).GreaterThan(-1);
            }
        }

        [Authenticate]
       // [RequiresAnyRole("")]
        public ListStockXListedItemResponse Get(ListStockXListedItemRequest request)
        {
        
              var Validation = new ValidateListStockXListedItem();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new ListStockXListedItemResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
                    AppUser User = this.GetCurrentAppUser();
            var findQl = Db.From<StockXListedItem>().Join<StockXAccount>((A, B) => A.AccountId == B.Id && A.UserId == B.UserId).Join<Inventory>((A, B) => A.SkuUuid == B.Sku && A.UserId == B.UserId && A.AccountId == B.StockXAccountId).Where(A => A.UserId == User.Id);
            var StockXListedItems = Db.SelectMulti<StockXListedItem, StockXAccount,Inventory>(findQl.OrderBy(A => A.Id).Skip(request.Skip).Take(50));
            var CountOf = Db.Count(findQl);
            return new ListStockXListedItemResponse()
            {
                Success = true,
                Total =CountOf,
                StockXListedItems = StockXListedItems

            };
        
        
        }
         private class ValidateUpdateStockXListedItem : AbstractValidator<UpdateStockXListedItemRequest>
        {
            public ValidateUpdateStockXListedItem()
            {
               

            }
        }
        [Authenticate]
       // [RequiresAnyRole("")]
        public UpdateStockXListedItemResponse Put(UpdateStockXListedItemRequest request)
        {
        
                   var Validation = new ValidateUpdateStockXListedItem();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new UpdateStockXListedItemResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
                         AppUser User = this.GetCurrentAppUser();
            var ExistingStockXListedItem = Db.Single<StockXListedItem>(A => A.UserId == User.Id && A.Id == request.StockXListedItemId);
            if(ExistingStockXListedItem ==null)
            {
                return new UpdateStockXListedItemResponse()
                {
                    Success = false,
                    Message = "No Such StockXListedItem"
                };
            }
            

            var TotalUpdated = Db.Update(ExistingStockXListedItem);
        
            return new UpdateStockXListedItemResponse()
            {
                TotalUpdated = TotalUpdated,
                Success = true,
               
            };
        }
          private class ValidateCreateStockXListedItem : AbstractValidator<CreateStockXListedItemRequest>
        {
            public ValidateCreateStockXListedItem()
            {
          

            }
        }
        
        [Authenticate]
       // [RequiresAnyRole("")]
        public CreateStockXListedItemResponse Post(CreateStockXListedItemRequest request)
        {
                          var Validation = new ValidateCreateStockXListedItem();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new CreateStockXListedItemResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
                   AppUser User = this.GetCurrentAppUser();
            var NewStockXListedItem = new StockXListedItem()
            {
          
  
            };
            var InsertedId = Db.Insert(NewStockXListedItem,true);
            return new CreateStockXListedItemResponse()
            {
                InsertedId=InsertedId,
                Success = true,
               
            };
        }
           private class ValidateDeleteStockXListedItem : AbstractValidator<DeleteStockXListedItemRequest>
        {
            public ValidateDeleteStockXListedItem()
            {

            }
        }
        [Authenticate]
       // [RequiresAnyRole("")]
        public DeleteStockXListedItemResponse Delete(DeleteStockXListedItemRequest request)
        {
                            var Validation = new ValidateDeleteStockXListedItem();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new DeleteStockXListedItemResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
            var ExistingStockXListedItem = Db.Single<StockXListedItem>(A => A.Id == request.StockXListedItemId);
            if (ExistingStockXListedItem == null)
            {
                return new DeleteStockXListedItemResponse()
                {
                    Success = false,
                    Message = "No Such StockXListedItem"
                };
            }
                    AppUser User = this.GetCurrentAppUser();
            var Deleted =Db.Delete<StockXListedItem>(Db.From<StockXListedItem>().Where(A=>A.Id== request.StockXListedItemId && A.UserId==User.Id));
            
            return new DeleteStockXListedItemResponse()
            {
                TotalDeleted = Deleted,
                Success = true,
               
            };
        }
        
        [Authenticate]
       // [RequiresAnyRole("")]
        public ListOneStockXListedItemResponse Get(ListOneStockXListedItemRequest request)
        {
         AppUser User = this.GetCurrentAppUser();
            var ExistingStockXListedItem = Db.Single<StockXListedItem>(A => User.Id == A.UserId && A.Id == request.StockXListedItemId);
            if(ExistingStockXListedItem ==null)
            {
                return new ListOneStockXListedItemResponse()
                {
                    Success = false,
                    Message = "No Such StockXListedItem"
                };
            }
            return new ListOneStockXListedItemResponse()
            {
                StockXListedItemItem = ExistingStockXListedItem,
                Success = true,
               
            };
        }

     

      
    }
}
    