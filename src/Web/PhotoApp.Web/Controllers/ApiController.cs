using Microsoft.AspNetCore.Mvc;
using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Controllers
{
    public class ApiController : Controller
    {
        private readonly PhotoAppDbContext dbContext;

        public ApiController(PhotoAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task AddLike(Like like)
        {
            dbContext.PhotosChallanges.Where(p => p.PhotoId == like.PhotoId).Where(c => c.ChallangeId == like.ChallangeId).FirstOrDefault().VotesCount++;

            UsersPhotoLikes usersPhotoLikes = new UsersPhotoLikes
            {
                PhotoId = like.PhotoId,
                UserId = like.UserId
            };

            dbContext.UsersPhotoLikes.Add(usersPhotoLikes);

            await dbContext.SaveChangesAsync();
        }
    }
}
