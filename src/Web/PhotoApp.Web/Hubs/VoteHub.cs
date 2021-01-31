using Microsoft.AspNetCore.SignalR;
using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Hubs
{
    public class VoteHub : Hub
    {
        private readonly PhotoAppDbContext dbContext;

        public VoteHub(PhotoAppDbContext dbContext
                        )
        {
            this.dbContext = dbContext;
        }

        
    }
}
