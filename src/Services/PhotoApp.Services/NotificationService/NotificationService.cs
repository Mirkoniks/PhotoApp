using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Services.ChallangeService;
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

        public async Task DismisNotification(string notificationId)
        {
            dbContext.Notifications.Where(n => n.IsDismissed).FirstOrDefault().IsDismissed = true;

            await dbContext.SaveChangesAsync();
        }

    }
}
