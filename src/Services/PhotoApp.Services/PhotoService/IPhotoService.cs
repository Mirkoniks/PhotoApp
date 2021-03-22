using Microsoft.AspNetCore.Http;
using PhotoApp.Services.Models.Photo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.PhotoService
{
    public interface IPhotoService
    {
        public Task<int> AddPhotoAsync(string photoLink);

        public Task<PhotoServiceModel> FindPhotoByIdAsync(int id);

        public Task AssingPhotoToChallangeAsync(AssignPhotoToChallangeServiceModel serviceModel);

        public Task<int> GetTotalPhotosCount();

        public Task<int> GetPhotosCountFromToday();

        public Task<string> GetPhotoUrl(int id);

        public Task<int> GetChallangeCoverPhotoId(int id);

        public Task ChangeCoverPhoto(IFormFile file, string userId);

        public Task ChangeProfilePhoto(IFormFile file, string userId);

    }
}
