using PhotoApp.Services.Models.Challange;
using PhotoApp.Services.Models.Photo;
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

        public Task<bool> IsNewDay(DateTime now);

        public Task<TopPhotosServiceModel> FirstTopPhotosFromChallange(int challangeId, int numPhotos);

        //if startPhotoId == 0, then take pic from begining
        public Task<TopPhotosServiceModel> GetTopPhotosFromChallange(int challangeId, int numPhotos, int startPhotoId = 0);

        public Task<TopPhotosServiceModel> GetTopPhotos(int numPhotos);

        public Task<TopPhotosServiceModel> GetLatestPhotos(int numPhotos);

        public Task<string> GetChallangeNameById(int id);

        public Task  RunChallagesCheckAsync();

        public Task<AllChallangesServiceModel> GetAllOpenChallanges();

        public Task<AllChallangesServiceModel> GetAllClosedCallanges();

        public Task<AllChallangesServiceModel> GetAllUpcomingChallanges();

        public Task SetChallangeCoverPhoto(int challangeId, int photoId);

        public Task RunChallageCheckAsync(int id);

        public Task EditChallange(EditChallangeServiceModel editChallangeServiceModel);

        public Task DeleteChallange(int id);

        public Task<UserLikedPhotosServiceModel> GetUserLikedPhotos(string userId);

        public Task<UserPhotosServiceModel> GetUserPhotos(string userId);

        public Task<bool> IsValidId(int id);


        public Task<int> GetUpcomigChallangesCount();

        public Task<int> GetOpenChallangesCount();

        public Task<int> GetClosedChallangesCount();

        public Task<AdminAllChallangesServiceModel> AdminGetAllChallanges();

        public Task<string> SetStatus(bool isOpen, bool isUpcoming);

        public Task<AdminChallangeServiceModel> GetChallangeById(int id);

    }
}
