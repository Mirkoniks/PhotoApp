using PhotoApp.Services.Models.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.NotificationService
{
    public interface INotificationService
    {
        public Task<int> Add(string message, int challageId);

        public Task AddToUser(int notificationId, string userId);

        public Task NotifyWinner(string userId, int challangeId);

        public Task DismisNotification(int notificationId);

        public Task<IEnumerable<NotificationServiceModel>> GetUserNotifcations(string userId);

        public Task<int> GetUserNotifcationsCount(string userId);

    }
}
