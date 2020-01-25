
using System.Collections.Generic;
namespace Funday.ServiceModel.DiscordNotifications
{
    public class CreateDiscordNotificationsResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public long InsertedId {get; set; }
    }
    
      public class UpdateDiscordNotificationsResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalUpdated {get; set;}
    }
    
      public class DeleteDiscordNotificationsResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalDeleted {get; set;}
    }
    
      public class ListDiscordNotificationsResponse 
    { 
        public long Total { get;set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<DiscordNotifications> DiscordNotificationss { get; set; }
    }
      public class ListOneDiscordNotificationsResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public DiscordNotifications DiscordNotificationsItem { get; set; }
    }
    
    
}
    