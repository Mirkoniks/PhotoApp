using PhotoApp.Services.Models.Challange;
using PhotoApp.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.ChallangeService
{
    public interface IChallangeService
    {
        public Task<int> CreateChallangeAsync(CreateChallangeServiceModel serviceModel);

        public AllChallangesServiceModel GetFisrtChallanges(int numOfChallanges);

        public Task<ChallangeServiceModel> FindChallangeById(int id);

        public Task AddPhotoToChallange(int photoId, int challangeId);

        public Task<PhotosChallangesServiceModel> SkipAndGetPhotosFromChallange(int skipStep, int numPhotos, int challangeId);
    }
}
