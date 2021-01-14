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

        public Task FindPhotoByIdAsync();

        public Task AssingPhotoToChallangeAsync(AssignPhotoToChallangeServiceModel serviceModel);
    }
}
