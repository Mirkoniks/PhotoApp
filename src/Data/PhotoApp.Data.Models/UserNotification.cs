using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Data.Models
{
    public class UserNotification
    {
        public string UserId { get; set; }

        public PhotoAppUser User { get; set; }

        public int NotificationId { get; set; }

        public Notification Notification { get; set; }
    }
}
