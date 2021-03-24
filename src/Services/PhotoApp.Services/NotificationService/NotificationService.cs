using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Services.ChallangeService;
using PhotoApp.Services.Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly PhotoAppDbContext dbContext;

        public NotificationService(PhotoAppDbContext dbContext
                                   )
        {
            this.dbContext = dbContext;
        }

        public async Task<int> Add(string message, int challageId)
        {
            Notification notification = new Notification()
            {
                Message = message,
                IsDismissed = false,
                ChallangeId = challageId
            };

            await dbContext.AddAsync(notification);

            await dbContext.SaveChangesAsync();

            return notification.Id;
        }

        public async Task AddToUser(int notificationId, string userId)
        {
            UserNotification userNotification = new UserNotification()
            {
                NotificationId = notificationId,
                UserId = userId
            };

            await dbContext.AddAsync(userNotification);

            await dbContext.SaveChangesAsync();
        }

        public async Task NotifyWinner(string userId, int challangeId)
        {
            var isAlreadyAdded = dbContext.Notifications.Any(n => n.ChallangeId == challangeId);

            if (!isAlreadyAdded)
            {
                var challangename = dbContext.Challanges.Where(c => c.ChallangeId == challangeId).FirstOrDefault().Name;

                string message = $"congratulations ! you have won the {challangename} challenge";

                var notificationId = await Add(message, challangeId);

                await AddToUser(notificationId, userId);
            }
        }

        public async Task DismisNotification(int notificationId)
        {
            dbContext.Notifications.Where(n => n.Id == notificationId).FirstOrDefault().IsDismissed = true;

            await dbContext.SaveChangesAsync();
        }


        public async Task<IEnumerable<NotificationServiceModel>> GetUserNotifcations(string userId)
        {
            var fromDb = dbContext.UserNotifications.Where(u => u.UserId == userId).Select(n => n.NotificationId).ToList();

            List<NotificationServiceModel> notificartions = new List<NotificationServiceModel>();

            foreach (var item in fromDb)
            {
                var isDismised = dbContext.Notifications.Where(n => n.Id == item).FirstOrDefault().IsDismissed;

                if (!isDismised)
                {
                    var challangeId = dbContext.Notifications.Where(c => c.Id == item).FirstOrDefault().ChallangeId;
                    var challageName = dbContext.Challanges.Where(c => c.ChallangeId == challangeId).FirstOrDefault().Name;

                    NotificationServiceModel model = new NotificationServiceModel()
                    {
                        ChallangeId = challangeId,
                        ChallangeName = challageName,
                        NotificationId = item
                    };

                    notificartions.Add(model);
                }
            }

            return notificartions;
        }

        public async Task<int> GetUserNotifcationsCount(string userId)
        {
            int counter = 0;
            var notifications = dbContext.UserNotifications.Where(un => un.UserId == userId).Select(un => un.NotificationId).ToList();

            foreach (var item in notifications)
            {
                var isDissmised = dbContext.Notifications.Where(n => n.Id == item).FirstOrDefault().IsDismissed;

                if (!isDissmised)
                {
                    counter++;
                }
            }

            return counter;
        }

    }
}
