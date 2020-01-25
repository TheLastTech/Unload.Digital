using ServiceStack;

namespace Funday.ServiceModel.DiscordNotifications
{
 

    [Route("/DiscordNotifications/update", "Put")]
    public class UpdateDiscordNotificationsRequest : IReturn<UpdateDiscordNotificationsResponse>
    {
        public long UserID { get; set; }
        public string Sold { get; set; }
        public string Listing { get; set; }
        public string Error { get; set; }

        public int DiscordNotificationsId { get; set; }
    }

    [Route("/DiscordNotifications/listone", "Get")]
    public class ListOneDiscordNotificationsRequest : IReturn<ListOneDiscordNotificationsResponse>
    {
        public int DiscordNotificationsId { get; set; }
    }
}