using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Data.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public bool IsDismissed { get; set; }

        public int ChallangeId { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; } = new HashSet<UserNotification>();
    }
}
