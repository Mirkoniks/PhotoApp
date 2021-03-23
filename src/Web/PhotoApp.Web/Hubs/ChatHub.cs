using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using PhotoApp.Data.Models;
using PhotoApp.Services.UserService;
using PhotoApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<PhotoAppUser> userManager;
        private readonly IUserService userService;

        public ChatHub(UserManager<PhotoAppUser> userManager,
                       IUserService userService)
        {
            this.userManager = userManager;
            this.userService = userService;
        }

        public async Task Send(MessageModel message)
        {
            var user =  await userService.GetUserById(message.FromUserId);

            MessageModel messageModel = new MessageModel()
            {
                Message = message.Message,
                NamesFrom = $"{user.FirstName} {user.LastName}"
            };

            await Clients.User(message.ToUserId).SendAsync("ReceiveMessage", messageModel);
        }
    }
}
