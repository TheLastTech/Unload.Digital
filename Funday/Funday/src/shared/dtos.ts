/* Options:
Date: 2020-01-20 01:00:52
Version: 5.81
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: https://localhost:5001

//GlobalNamespace: 
//AddServiceStackTypes: True
//AddResponseStatus: False
//AddImplicitVersion: 
//AddDescriptionAsComments: True
//IncludeTypes: 
//ExcludeTypes: 
//DefaultImports: 
*/


export interface IReturn<T>
{
    createResponse(): T;
}

export interface IReturnVoid
{
    createResponse(): void;
}

export interface IHasSessionId
{
    SessionId: string;
}

export interface IHasBearerToken
{
    BearerToken: string;
}

export interface IPost
{
}

export class StockXAccount
{
    public Id: number;
    public Email: string;
    public Password: string;
    public ProxyUsername: string;
    public ProxyPassword: string;
    public ProxyHost: string;
    public ProxyPort: number;
    public ProxyActive: boolean;
    public Active: boolean;
    public CustomerID: number;
    public Currency: string;
    public Country: string;
    public Token: string;
    public UserId: number;
    public Verified: boolean;
    public NextVerification: string;
    public LoginFails: number;
    public Disabled: boolean;
    public AccountThread: string;
    public NextAccountInteraction: string;

    public constructor(init?: Partial<StockXAccount>) { (Object as any).assign(this, init); }
}

export class Tuple_2<T1, T2>
{
    public Item1: T1;
    public Item2: T2;

    public constructor(init?: Partial<Tuple_2<T1, T2>>) { (Object as any).assign(this, init); }
}

export class Inventory
{
    public Id: number;
    public UserId: number;
    public StockXUrl: string;
    public Quantity: number;
    public MinSell: number;
    public StartingAsk: number;
    public Size: string;
    public Active: boolean;
    public StockXAccountId: number;
    public Status: string;
    public Sku: string;
    public ParentSku: string;

    public constructor(init?: Partial<Inventory>) { (Object as any).assign(this, init); }
}

export class Tuple_3<T1, T2, T3>
{
    public Item1: T1;
    public Item2: T2;
    public Item3: T3;

    public constructor(init?: Partial<Tuple_3<T1, T2, T3>>) { (Object as any).assign(this, init); }
}

export class Tracking
{
    public Number: string;
    public Status: string;
    public LabelUrl: string;
    public FormUrl: string;
    public TrackUrl: string;
    public DeliveryDate: string;
    public DeliveryDateTime: boolean;

    public constructor(init?: Partial<Tracking>) { (Object as any).assign(this, init); }
}

export class PortfolioItemMeta
{
    public MerchantSku: string;
    public DiscountCode: string;
    public SizePreferences: string;

    public constructor(init?: Partial<PortfolioItemMeta>) { (Object as any).assign(this, init); }
}

export class Breadcrumb
{
    public Level: number;
    public Name: string;
    public Url: string;
    public IsBrand: number;

    public constructor(init?: Partial<Breadcrumb>) { (Object as any).assign(this, init); }
}

export class Media
{
    public ImageUrl: string;
    public SmallImageUrl: string;
    public ThumbUrl: string;
    public Gallery: string[];
    public Hidden: boolean;
    public Has360: boolean;
    public Key360: string;

    public constructor(init?: Partial<Media>) { (Object as any).assign(this, init); }
}

export class Trait
{
    public Name: string;
    public Value: string;
    public Filterable: boolean;
    public Visible: boolean;
    public Highlight: boolean;
    public Format: string;

    public constructor(init?: Partial<Trait>) { (Object as any).assign(this, init); }
}

export class ProductMeta
{
    public Charity: boolean;
    public Raffle: boolean;
    public MobileOnly: boolean;
    public Restock: boolean;
    public Deleted: boolean;
    public Hidden: boolean;
    public LockBuying: boolean;
    public LockSelling: boolean;
    public Redirected: boolean;

    public constructor(init?: Partial<ProductMeta>) { (Object as any).assign(this, init); }
}

export class Address
{
    public CountryCodeAlpha2: string;
    public ExtendedAddress: number;
    public FirstName: string;
    public LastName: string;
    public Locality: string;
    public PostalCode: number;
    public Region: string;
    public StreetAddress: string;
    public Telephone: string;

    public constructor(init?: Partial<Address>) { (Object as any).assign(this, init); }
}

export class Shipping
{
    public Address: Address;

    public constructor(init?: Partial<Shipping>) { (Object as any).assign(this, init); }
}

export class EnhancedImage
{
    public ProductUuid: string;
    public ImageHost: string;
    public ImageKey: string;
    public ImageCount: number;
    public UpdatedAt: string;
    public CreatedAt: string;

    public constructor(init?: Partial<EnhancedImage>) { (Object as any).assign(this, init); }
}

export class Children
{

    public constructor(init?: Partial<Children>) { (Object as any).assign(this, init); }
}

export class Market
{
    public ProductId: number;
    public SkuUuid: string;
    public ProductUuid: string;
    public LowestAsk: number;
    public LowestAskFloat: number;
    public LowestAskSize: string;
    public ParentLowestAsk: number;
    public NumberOfAsks: number;
    public SalesThisPeriod: number;
    public SalesLastPeriod: number;
    public HighestBid: number;
    public HighestBidFloat: number;
    public HighestBidSize: number;
    public NumberOfBids: number;
    public AnnualHigh: number;
    public AnnualLow: number;
    public DeadstockRangeLow: number;
    public DeadstockRangeHigh: number;
    public Volatility: number;
    public DeadstockSold: number;
    public PricePremium: number;
    public AverageDeadstockPrice: number;
    public LastSale: number;
    public LastSaleSize: number;
    public SalesLast72Hours: number;
    public ChangeValue: number;
    public ChangePercentage: number;
    public AbsChangePercentage: number;
    public TotalDollars: number;
    public UpdatedAt: number;
    public LastLowestAskTime: number;
    public LastHighestBidTime: number;
    public LastSaleDate: string;
    public CreatedAt: string;
    public DeadstockSoldRank: number;
    public PricePremiumRank: number;
    public AverageDeadstockPriceRank: number;
    public Featured: number;

    public constructor(init?: Partial<Market>) { (Object as any).assign(this, init); }
}

export class Gtin
{
    public Id: string;
    public Type: string;
    public SkuId: string;
    public IdentifierValue: string;

    public constructor(init?: Partial<Gtin>) { (Object as any).assign(this, init); }
}

export class Product
{
    public Id: string;
    public Uuid: string;
    public Brand: string;
    public Breadcrumbs: Breadcrumb[];
    public Category: string;
    public CharityCondition: number;
    public Colorway: string;
    public Condition: string;
    public CountryOfManufacture: string;
    public Description: string;
    public Gender: string;
    public ContentGroup: string;
    public IpoDate: string;
    public MinimumBid: number;
    public Doppelgangers: Object[];
    public Media: Media;
    public Name: string;
    public IsLithiumIonBattery: boolean;
    public Traits: Trait[];
    public PortfolioItems: Object[];
    public PrimaryCategory: string;
    public SecondaryCategory: string;
    public UsHtsCode: string;
    public UsHtsDescription: string;
    public ProductCategory: string;
    public ReleaseDate: string;
    public RetailPrice: number;
    public Shoe: string;
    public ShortDescription: string;
    public StyleId: string;
    public TickerSymbol: string;
    public Title: string;
    public DataType: string;
    public Type: number;
    public SizeTitle: string;
    public SizeDescriptor: string;
    public SizeAllDescriptor: string;
    public Url: string;
    public UrlKey: string;
    public Year: number;
    public ShippingGroup: string;
    public Hold: boolean;
    public ALim: number;
    public Meta: ProductMeta;
    public Shipping: Shipping;
    public EnhancedImage: EnhancedImage;
    public Children: Children;
    public ParentId: string;
    public ParentUuid: string;
    public SizeSortOrder: number;
    public ShoeSize: string;
    public Market: Market;
    public Gtins: Gtin[];
    public Upc: string;
    public UpcValid: boolean;

    public constructor(init?: Partial<Product>) { (Object as any).assign(this, init); }
}

export class Merchant
{
    public AccountName: Object;
    public MerchantId: number;
    public PaypalEmail: string;
    public PreferredPayout: number;

    public constructor(init?: Partial<Merchant>) { (Object as any).assign(this, init); }
}

export class Customer
{
    public Id: number;
    public Uuid: string;
    public FirstName: string;
    public LastName: string;
    public FullName: string;
    public Email: string;
    public Username: string;
    public EmailVerified: boolean;
    public DefaultSize: string;
    public DefaultCategory: string;
    public DefaultCurrency: string;
    public Language: string;
    public Categories: string[];
    public VacationDate: string;
    public IsActive: boolean;
    public Flagged: boolean;
    public HidePortfolioBanner: boolean;
    public ReferUrl: string;
    public CreatedAt: string;
    public CreatedAtTime: number;
    public IsTrader: boolean;
    public HasBuyerReward: boolean;
    public GdprStatus: string;

    public constructor(init?: Partial<Customer>) { (Object as any).assign(this, init); }
}

export class StockXListedItem
{
    public Id: number;
    public UserId: number;
    public ChainId: string;
    public CustomerId: number;
    public UserUuid: string;
    public InventoryId: string;
    public ProductId: string;
    public SkuUuid: string;
    public MerchantId: number;
    public Condition: number;
    public Action: number;
    public ActionBy: number;
    public Amount: number;
    public LocalCurrency: string;
    public LocalAmount: number;
    public LocalExchangeRate: number;
    public BidAskSpread: number;
    public ExpiresAt: string;
    public ExpiresAtTime: number;
    public FaqLink: string;
    public GainLossDollars: number;
    public GainLossPercentage: number;
    public MarketValue: number;
    public MatchedWith: string;
    public MatchedState: number;
    public MatchedWithDate: string;
    public Owner: string;
    public UserFullname: string;
    public PurchasePrice: number;
    public PurchaseDate: string;
    public PurchaseDateTime: number;
    public ShipByDate: string;
    public State: number;
    public StatusMessage: string;
    public Text: string;
    public Notes: string;
    public CountryCode: string;
    public CreatedAt: string;
    public CreatedAtTime: number;
    public Url: string;
    public ReferUrl: string;
    public CanEdit: boolean;
    public CanDelete: boolean;
    public IsPartOfBulkShipment: boolean;
    public Tracking: Tracking;
    public Meta: PortfolioItemMeta;
    public OrderNumber: string;
    public Total: string;
    public LocalTotal: string;
    public Product: Product;
    public Merchant: Merchant;
    public Customer: Customer;
    public LocalMarketValue: number;
    public LocalGainLoss: number;

    public constructor(init?: Partial<StockXListedItem>) { (Object as any).assign(this, init); }
}

// @DataContract
export class ResponseError
{
    // @DataMember(Order=1, EmitDefaultValue=false)
    public ErrorCode: string;

    // @DataMember(Order=2, EmitDefaultValue=false)
    public FieldName: string;

    // @DataMember(Order=3, EmitDefaultValue=false)
    public Message: string;

    // @DataMember(Order=4, EmitDefaultValue=false)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<ResponseError>) { (Object as any).assign(this, init); }
}

// @DataContract
export class ResponseStatus
{
    // @DataMember(Order=1)
    public ErrorCode: string;

    // @DataMember(Order=2)
    public Message: string;

    // @DataMember(Order=3)
    public StackTrace: string;

    // @DataMember(Order=4)
    public Errors: ResponseError[];

    // @DataMember(Order=5)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<ResponseStatus>) { (Object as any).assign(this, init); }
}

export class UserAuth
{
    public Id: number;
    public UserName: string;
    public Email: string;
    public PrimaryEmail: string;
    public PhoneNumber: string;
    public FirstName: string;
    public LastName: string;
    public DisplayName: string;
    public Company: string;
    public BirthDate: string;
    public BirthDateRaw: string;
    public Address: string;
    public Address2: string;
    public City: string;
    public State: string;
    public Country: string;
    public Culture: string;
    public FullName: string;
    public Gender: string;
    public Language: string;
    public MailAddress: string;
    public Nickname: string;
    public PostalCode: string;
    public TimeZone: string;
    public Salt: string;
    public PasswordHash: string;
    public DigestHa1Hash: string;
    public Roles: string[];
    public Permissions: string[];
    public CreatedDate: string;
    public ModifiedDate: string;
    public InvalidLoginAttempts: number;
    public LastLoginAttempt: string;
    public LockedDate: string;
    public RecoveryToken: string;
    public RefId: number;
    public RefIdStr: string;
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<UserAuth>) { (Object as any).assign(this, init); }
}

export class AppUser extends UserAuth
{
    public ProfileUrl: string;
    public LastLoginIp: string;
    public LastLoginDate: string;

    public constructor(init?: Partial<AppUser>) { super(init); (Object as any).assign(this, init); }
}

export class HelloResponse
{
    public Result: string;

    public constructor(init?: Partial<HelloResponse>) { (Object as any).assign(this, init); }
}

export class ListStockXAccountResponse
{
    public Total: number;
    public Success: boolean;
    public Message: string;
    public StockXAccounts: StockXAccount[];

    public constructor(init?: Partial<ListStockXAccountResponse>) { (Object as any).assign(this, init); }
}

export class UpdateStockXAccountResponse
{
    public Success: boolean;
    public Message: string;
    public TotalUpdated: number;

    public constructor(init?: Partial<UpdateStockXAccountResponse>) { (Object as any).assign(this, init); }
}

export class CreateStockXAccountResponse
{
    public Success: boolean;
    public Message: string;
    public InsertedId: number;

    public constructor(init?: Partial<CreateStockXAccountResponse>) { (Object as any).assign(this, init); }
}

export class DeleteStockXAccountResponse
{
    public Success: boolean;
    public Message: string;
    public TotalDeleted: number;

    public constructor(init?: Partial<DeleteStockXAccountResponse>) { (Object as any).assign(this, init); }
}

export class ListOneStockXAccountResponse
{
    public Success: boolean;
    public Message: string;
    public StockXAccountItem: StockXAccount;

    public constructor(init?: Partial<ListOneStockXAccountResponse>) { (Object as any).assign(this, init); }
}

export class ListInventoryResponse
{
    public Total: number;
    public Success: boolean;
    public Message: string;
    public Inventorys: Array<Tuple_2<Inventory, StockXAccount>>;

    public constructor(init?: Partial<ListInventoryResponse>) { (Object as any).assign(this, init); }
}

export class UpdateInventoryResponse
{
    public Success: boolean;
    public Message: string;
    public TotalUpdated: number;

    public constructor(init?: Partial<UpdateInventoryResponse>) { (Object as any).assign(this, init); }
}

export class CreateInventoryResponse
{
    public Success: boolean;
    public Message: string;
    public InsertedId: number;

    public constructor(init?: Partial<CreateInventoryResponse>) { (Object as any).assign(this, init); }
}

export class DeleteInventoryResponse
{
    public Success: boolean;
    public Message: string;
    public TotalDeleted: number;

    public constructor(init?: Partial<DeleteInventoryResponse>) { (Object as any).assign(this, init); }
}

export class ListOneInventoryResponse
{
    public Success: boolean;
    public Message: string;
    public InventoryItem: Inventory;

    public constructor(init?: Partial<ListOneInventoryResponse>) { (Object as any).assign(this, init); }
}

export class ListStockXListedItemResponse
{
    public Total: number;
    public Success: boolean;
    public Message: string;
    public StockXListedItems: Array<Tuple_3<StockXListedItem, AppUser, Inventory>>;

    public constructor(init?: Partial<ListStockXListedItemResponse>) { (Object as any).assign(this, init); }
}

export class UpdateStockXListedItemResponse
{
    public Success: boolean;
    public Message: string;
    public TotalUpdated: number;

    public constructor(init?: Partial<UpdateStockXListedItemResponse>) { (Object as any).assign(this, init); }
}

export class CreateStockXListedItemResponse
{
    public Success: boolean;
    public Message: string;
    public InsertedId: number;

    public constructor(init?: Partial<CreateStockXListedItemResponse>) { (Object as any).assign(this, init); }
}

export class DeleteStockXListedItemResponse
{
    public Success: boolean;
    public Message: string;
    public TotalDeleted: number;

    public constructor(init?: Partial<DeleteStockXListedItemResponse>) { (Object as any).assign(this, init); }
}

export class ListOneStockXListedItemResponse
{
    public Success: boolean;
    public Message: string;
    public StockXListedItemItem: StockXListedItem;

    public constructor(init?: Partial<ListOneStockXListedItemResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class AuthenticateResponse implements IHasSessionId, IHasBearerToken
{
    // @DataMember(Order=1)
    public UserId: string;

    // @DataMember(Order=2)
    public SessionId: string;

    // @DataMember(Order=3)
    public UserName: string;

    // @DataMember(Order=4)
    public DisplayName: string;

    // @DataMember(Order=5)
    public ReferrerUrl: string;

    // @DataMember(Order=6)
    public BearerToken: string;

    // @DataMember(Order=7)
    public RefreshToken: string;

    // @DataMember(Order=8)
    public ProfileUrl: string;

    // @DataMember(Order=9)
    public Roles: string[];

    // @DataMember(Order=10)
    public Permissions: string[];

    // @DataMember(Order=11)
    public ResponseStatus: ResponseStatus;

    // @DataMember(Order=12)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<AuthenticateResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class AssignRolesResponse
{
    // @DataMember(Order=1)
    public AllRoles: string[];

    // @DataMember(Order=2)
    public AllPermissions: string[];

    // @DataMember(Order=3)
    public Meta: { [index: string]: string; };

    // @DataMember(Order=4)
    public ResponseStatus: ResponseStatus;

    public constructor(init?: Partial<AssignRolesResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class UnAssignRolesResponse
{
    // @DataMember(Order=1)
    public AllRoles: string[];

    // @DataMember(Order=2)
    public AllPermissions: string[];

    // @DataMember(Order=3)
    public Meta: { [index: string]: string; };

    // @DataMember(Order=4)
    public ResponseStatus: ResponseStatus;

    public constructor(init?: Partial<UnAssignRolesResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class ConvertSessionToTokenResponse
{
    // @DataMember(Order=1)
    public Meta: { [index: string]: string; };

    // @DataMember(Order=2)
    public AccessToken: string;

    // @DataMember(Order=3)
    public RefreshToken: string;

    // @DataMember(Order=4)
    public ResponseStatus: ResponseStatus;

    public constructor(init?: Partial<ConvertSessionToTokenResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class GetAccessTokenResponse
{
    // @DataMember(Order=1)
    public AccessToken: string;

    // @DataMember(Order=2)
    public Meta: { [index: string]: string; };

    // @DataMember(Order=3)
    public ResponseStatus: ResponseStatus;

    public constructor(init?: Partial<GetAccessTokenResponse>) { (Object as any).assign(this, init); }
}

// @Route("/hello")
// @Route("/hello/{Name}")
export class Hello implements IReturn<HelloResponse>
{
    public Name: string;

    public constructor(init?: Partial<Hello>) { (Object as any).assign(this, init); }
    public createResponse() { return new HelloResponse(); }
    public getTypeName() { return 'Hello'; }
}

// @Route("/StockXAccount/list", "Get")
export class ListStockXAccountRequest implements IReturn<ListStockXAccountResponse>
{
    public Skip: number;

    public constructor(init?: Partial<ListStockXAccountRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new ListStockXAccountResponse(); }
    public getTypeName() { return 'ListStockXAccountRequest'; }
}

// @Route("/StockXAccount/update", "Put")
export class UpdateStockXAccountRequest implements IReturn<UpdateStockXAccountResponse>
{
    public Email: string;
    public Password: string;
    public ProxyUsername: string;
    public ProxyPassword: string;
    public ProxyHost: string;
    public ProxyPort: number;
    public ProxyActive: boolean;
    public Active: boolean;
    public CustomerID: number;
    public Currency: string;
    public Country: string;
    public UserAgent: string;
    public Token: string;
    public StockXAccountId: number;
    public Disabled: boolean;

    public constructor(init?: Partial<UpdateStockXAccountRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new UpdateStockXAccountResponse(); }
    public getTypeName() { return 'UpdateStockXAccountRequest'; }
}

// @Route("/StockXAccount/create", "Post")
export class CreateStockXAccountRequest implements IReturn<CreateStockXAccountResponse>
{
    public Email: string;
    public Password: string;
    public ProxyUsername: string;
    public ProxyPassword: string;
    public ProxyHost: string;
    public ProxyPort: number;
    public ProxyActive: boolean;
    public Active: boolean;
    public CustomerID: number;
    public Currency: string;
    public Country: string;
    public UserAgent: string;
    public Token: string;

    public constructor(init?: Partial<CreateStockXAccountRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new CreateStockXAccountResponse(); }
    public getTypeName() { return 'CreateStockXAccountRequest'; }
}

// @Route("/StockXAccount/delete", "Delete")
export class DeleteStockXAccountRequest implements IReturn<DeleteStockXAccountResponse>
{
    public StockXAccountId: number;

    public constructor(init?: Partial<DeleteStockXAccountRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new DeleteStockXAccountResponse(); }
    public getTypeName() { return 'DeleteStockXAccountRequest'; }
}

// @Route("/StockXAccount/listone", "Get")
export class ListOneStockXAccountRequest implements IReturn<ListOneStockXAccountResponse>
{
    public StockXAccountId: number;

    public constructor(init?: Partial<ListOneStockXAccountRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new ListOneStockXAccountResponse(); }
    public getTypeName() { return 'ListOneStockXAccountRequest'; }
}

// @Route("/Inventory/list", "Get")
export class ListInventoryRequest implements IReturn<ListInventoryResponse>
{
    public Skip: number;

    public constructor(init?: Partial<ListInventoryRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new ListInventoryResponse(); }
    public getTypeName() { return 'ListInventoryRequest'; }
}

// @Route("/Inventory/update", "Put")
export class UpdateInventoryRequest implements IReturn<UpdateInventoryResponse>
{
    public StockXUrl: string;
    public Quantity: number;
    public MinSell: number;
    public StartingAsk: number;
    public Size: string;
    public Active: boolean;
    public StockXAccountId: number;
    public DateSold: string;
    public Status: string;
    public Sku: string;
    public InventoryId: number;

    public constructor(init?: Partial<UpdateInventoryRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new UpdateInventoryResponse(); }
    public getTypeName() { return 'UpdateInventoryRequest'; }
}

// @Route("/Inventory/create", "Post")
export class CreateInventoryRequest implements IReturn<CreateInventoryResponse>
{
    public StockXUrl: string;
    public Quantity: number;
    public MinSell: number;
    public StartingAsk: number;
    public Size: string;
    public Active: boolean;
    public StockXAccountId: number;
    public DateSold: string;
    public Status: string;
    public Sku: string;

    public constructor(init?: Partial<CreateInventoryRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new CreateInventoryResponse(); }
    public getTypeName() { return 'CreateInventoryRequest'; }
}

// @Route("/Inventory/toggle", "Post")
export class ToggleInventoryRequest implements IReturn<DeleteInventoryResponse>
{
    public InventoryId: number;

    public constructor(init?: Partial<ToggleInventoryRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new DeleteInventoryResponse(); }
    public getTypeName() { return 'ToggleInventoryRequest'; }
}

// @Route("/Inventory/delete", "Delete")
export class DeleteInventoryRequest implements IReturn<DeleteInventoryResponse>
{
    public InventoryId: number;

    public constructor(init?: Partial<DeleteInventoryRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new DeleteInventoryResponse(); }
    public getTypeName() { return 'DeleteInventoryRequest'; }
}

// @Route("/Inventory/listone", "Get")
export class ListOneInventoryRequest implements IReturn<ListOneInventoryResponse>
{
    public InventoryId: number;

    public constructor(init?: Partial<ListOneInventoryRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new ListOneInventoryResponse(); }
    public getTypeName() { return 'ListOneInventoryRequest'; }
}

// @Route("/StockXListedItem/list", "Get")
export class ListStockXListedItemRequest implements IReturn<ListStockXListedItemResponse>
{
    public Skip: number;

    public constructor(init?: Partial<ListStockXListedItemRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new ListStockXListedItemResponse(); }
    public getTypeName() { return 'ListStockXListedItemRequest'; }
}

// @Route("/StockXListedItem/update", "Put")
export class UpdateStockXListedItemRequest implements IReturn<UpdateStockXListedItemResponse>
{
    public StockXUrl: string;
    public StockXListedItemId: number;

    public constructor(init?: Partial<UpdateStockXListedItemRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new UpdateStockXListedItemResponse(); }
    public getTypeName() { return 'UpdateStockXListedItemRequest'; }
}

// @Route("/StockXListedItem/create", "Post")
export class CreateStockXListedItemRequest implements IReturn<CreateStockXListedItemResponse>
{
    public StockXUrl: string;

    public constructor(init?: Partial<CreateStockXListedItemRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new CreateStockXListedItemResponse(); }
    public getTypeName() { return 'CreateStockXListedItemRequest'; }
}

// @Route("/StockXListedItem/delete", "Delete")
export class DeleteStockXListedItemRequest implements IReturn<DeleteStockXListedItemResponse>
{
    public StockXListedItemId: number;

    public constructor(init?: Partial<DeleteStockXListedItemRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new DeleteStockXListedItemResponse(); }
    public getTypeName() { return 'DeleteStockXListedItemRequest'; }
}

// @Route("/StockXListedItem/listone", "Get")
export class ListOneStockXListedItemRequest implements IReturn<ListOneStockXListedItemResponse>
{
    public StockXListedItemId: number;

    public constructor(init?: Partial<ListOneStockXListedItemRequest>) { (Object as any).assign(this, init); }
    public createResponse() { return new ListOneStockXListedItemResponse(); }
    public getTypeName() { return 'ListOneStockXListedItemRequest'; }
}

// @Route("/auth")
// @Route("/auth/{provider}")
// @DataContract
export class Authenticate implements IReturn<AuthenticateResponse>, IPost
{
    // @DataMember(Order=1)
    public provider: string;

    // @DataMember(Order=2)
    public State: string;

    // @DataMember(Order=3)
    public oauth_token: string;

    // @DataMember(Order=4)
    public oauth_verifier: string;

    // @DataMember(Order=5)
    public UserName: string;

    // @DataMember(Order=6)
    public Password: string;

    // @DataMember(Order=7)
    public RememberMe: boolean;

    // @DataMember(Order=8)
    public Continue: string;

    // @DataMember(Order=9)
    public ErrorView: string;

    // @DataMember(Order=10)
    public nonce: string;

    // @DataMember(Order=11)
    public uri: string;

    // @DataMember(Order=12)
    public response: string;

    // @DataMember(Order=13)
    public qop: string;

    // @DataMember(Order=14)
    public nc: string;

    // @DataMember(Order=15)
    public cnonce: string;

    // @DataMember(Order=16)
    public UseTokenCookie: boolean;

    // @DataMember(Order=17)
    public AccessToken: string;

    // @DataMember(Order=18)
    public AccessTokenSecret: string;

    // @DataMember(Order=19)
    public scope: string;

    // @DataMember(Order=20)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<Authenticate>) { (Object as any).assign(this, init); }
    public createResponse() { return new AuthenticateResponse(); }
    public getTypeName() { return 'Authenticate'; }
}

// @Route("/assignroles")
// @DataContract
export class AssignRoles implements IReturn<AssignRolesResponse>, IPost
{
    // @DataMember(Order=1)
    public UserName: string;

    // @DataMember(Order=2)
    public Permissions: string[];

    // @DataMember(Order=3)
    public Roles: string[];

    // @DataMember(Order=4)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<AssignRoles>) { (Object as any).assign(this, init); }
    public createResponse() { return new AssignRolesResponse(); }
    public getTypeName() { return 'AssignRoles'; }
}

// @Route("/unassignroles")
// @DataContract
export class UnAssignRoles implements IReturn<UnAssignRolesResponse>, IPost
{
    // @DataMember(Order=1)
    public UserName: string;

    // @DataMember(Order=2)
    public Permissions: string[];

    // @DataMember(Order=3)
    public Roles: string[];

    // @DataMember(Order=4)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<UnAssignRoles>) { (Object as any).assign(this, init); }
    public createResponse() { return new UnAssignRolesResponse(); }
    public getTypeName() { return 'UnAssignRoles'; }
}

// @Route("/session-to-token")
// @DataContract
export class ConvertSessionToToken implements IReturn<ConvertSessionToTokenResponse>, IPost
{
    // @DataMember(Order=1)
    public PreserveSession: boolean;

    // @DataMember(Order=2)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<ConvertSessionToToken>) { (Object as any).assign(this, init); }
    public createResponse() { return new ConvertSessionToTokenResponse(); }
    public getTypeName() { return 'ConvertSessionToToken'; }
}

// @Route("/access-token")
// @DataContract
export class GetAccessToken implements IReturn<GetAccessTokenResponse>, IPost
{
    // @DataMember(Order=1)
    public RefreshToken: string;

    // @DataMember(Order=2)
    public UseTokenCookie: boolean;

    // @DataMember(Order=3)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<GetAccessToken>) { (Object as any).assign(this, init); }
    public createResponse() { return new GetAccessTokenResponse(); }
    public getTypeName() { return 'GetAccessToken'; }
}

