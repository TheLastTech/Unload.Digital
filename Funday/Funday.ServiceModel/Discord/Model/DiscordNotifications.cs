using ServiceStack.DataAnnotations;

namespace Funday.ServiceModel.DiscordNotifications
{
    public class DiscordNotifications
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Sold { get; set; }
        public string Listing { get; set; }
   public string Error { get; set; }
        public int Errors { get; set; }
        public bool Disabled { get; set; }
    }
}