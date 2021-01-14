using Microsoft.AspNetCore.SignalR;
using PhotoApp.Data;
using PhotoApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Hubs
{
    public class LoadHub : Hub
    {
        private readonly PhotoAppDbContext dbContext;

        public LoadHub(PhotoAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

            
        public async Task Send()
        {
            string link = dbContext.Photos.First().Link;

            await this.Clients.Caller.SendAsync(
                "Load",
                 link
                );
        }
    }
}
