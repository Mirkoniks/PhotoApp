using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Services.Models.Challange;
using PhotoApp.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace PhotoApp.Services.ChallangeService
{
    public class ChallangeService : IChallangeService
    {
        public static DateTime TodayDate { get; set; }

        private readonly PhotoAppDbContext dbContext;

        public ChallangeService(PhotoAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> CreateChallangeAsync(CreateChallangeServiceModel serviceModel)
        {
            Challange challange = new Challange
            {
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                IsOpen = serviceModel.IsOpen,
                StartTime = serviceModel.StartTime,
                EndTime = serviceModel.EndTime,
                MaxPhotos = serviceModel.MaxPhotos
            };

            int challangeId = challange.ChallangeId;

            await dbContext.Challanges.AddAsync(challange);
            await dbContext.SaveChangesAsync();

            return challangeId;
        }

        public AllChallangesServiceModel GetFisrtChallanges(int numOfChallanges)
        {
            IEnumerable<Challange> challanges = dbContext.Challanges.Take(numOfChallanges).ToList();

            AllChallangesServiceModel serviceModel = new AllChallangesServiceModel();
            List<ChallangeServiceModel> challangesList = new List<ChallangeServiceModel>();

            foreach (var item in challanges)
            {
                ChallangeServiceModel challange = new ChallangeServiceModel
                {
                    Id = item.ChallangeId,
                    Name = item.Name,
                    Description = item.Description,
                    IsOpen = item.IsOpen,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                };
                challangesList.Add(challange);
            }

            serviceModel.Challanges = challangesList;

            return serviceModel;
        }

        public async Task<ChallangeServiceModel> FindChallangeById(int id)
        {
            Challange challange = dbContext.Challanges.Where(c => c.ChallangeId == id).FirstOrDefault();

            ChallangeServiceModel challangeServiceModel = new ChallangeServiceModel
            {
                Id = challange.ChallangeId,
                Name = challange.Name,
                Description = challange.Description,
                IsOpen = challange.IsOpen,
                StartTime = challange.StartTime,
                EndTime = challange.EndTime,
            };

            return challangeServiceModel;
        }

        public async Task AddPhotoToChallange(int photoId, int challangeId)
        {
            PhotoChallange photoChallange = new PhotoChallange()
            {
                ChallangeId = challangeId,
                PhotoId = photoId
            };

            await dbContext.PhotosChallanges.AddAsync(photoChallange);

            await dbContext.SaveChangesAsync();
        }

        public async Task<PhotosChallangesServiceModel> SkipAndGetPhotosFromChallange(int skipStep, int numPhotos, int challangeId)
        {
            IEnumerable<PhotoChallange> photoChallanges = dbContext.PhotosChallanges
                                                                   .Where(c => c.ChallangeId == challangeId)
                                                                   .Skip(skipStep).Take(numPhotos)
                                                                   .ToList();

            PhotosChallangesServiceModel photosChallangesServiceModel = new PhotosChallangesServiceModel();

            List<PhotoChallangeServiceModel> serviceModels = new List<PhotoChallangeServiceModel>();

            foreach (var item in photoChallanges)
            {
                PhotoChallangeServiceModel serviceModel = new PhotoChallangeServiceModel()
                {
                    PhotoId = item.PhotoId,
                    ChallangeId = item.ChallangeId,
                    VoteCount = item.VotesCount
                };

                serviceModels.Add(serviceModel);
            }

            photosChallangesServiceModel.PhotosChallanges = serviceModels;

            return photosChallangesServiceModel;
        }

        public async Task<bool> IsNewDay(DateTime now)
        {
            DateTime dateNow = DateTime.Today;

            if (dateNow < TodayDate)
            {
                return true;
            }
            return false;
        }

        public async Task<TopPhotosServiceModel> FirstTopPhotosFromChallange(int challangeId, int numPhotos)
        {
            var photos = dbContext.PhotosChallanges.OrderByDescending(v => v.VotesCount).Where(c => c.ChallangeId == challangeId).Take(numPhotos).ToList();

            TopPhotosServiceModel serviceModel = new TopPhotosServiceModel();
            List<TopPhotoServiceModel> photoServiceModels = new List<TopPhotoServiceModel>();


            foreach (var photo in photos)
            {
                TopPhotoServiceModel photoServiceModel = new TopPhotoServiceModel();

                photoServiceModel.VotesCount = photo.VotesCount;
                photoServiceModel.PhotoLink =  GetPhotoLinkById(photo.PhotoId);
                photoServiceModel.UserId = GetUserIdByPicureId(photo.PhotoId);

                photoServiceModels.Add(photoServiceModel);
            }

            serviceModel.ChallangeId = challangeId;
            serviceModel.Photos = photoServiceModels;

            return serviceModel;
        }

        public async Task<TopPhotosServiceModel> GetTopPhotos(int numPhotos)
        {
            var photos = dbContext.PhotosChallanges.OrderByDescending(v => v.VotesCount).Take(numPhotos).ToList();

            TopPhotosServiceModel serviceModel = new TopPhotosServiceModel();
            List<TopPhotoServiceModel> photoServiceModels = new List<TopPhotoServiceModel>();

            foreach (var photo in photos)
            {
                TopPhotoServiceModel photoServiceModel = new TopPhotoServiceModel();

                photoServiceModel.VotesCount = photo.VotesCount;
                photoServiceModel.PhotoLink = GetPhotoLinkById(photo.PhotoId);
                photoServiceModel.UserId = GetUserIdByPicureId(photo.PhotoId);

                photoServiceModels.Add(photoServiceModel);
            }

            serviceModel.Photos = photoServiceModels;

            return serviceModel;
        }

        public async Task<TopPhotosServiceModel> GetTopPhotosFromChallange(int challangeId, int numPhotos, int startPhotoId = 0)
        {
            throw new NotImplementedException();
        }

        private string GetPhotoLinkById(int id)
        {
            return dbContext.Photos.Where(p => p.PhotoId == id).FirstOrDefault().Link;
        }

        private string GetUserIdByPicureId(int id)
        {
            var result =  dbContext.UsersPhotos.Where(p => p.PhotoId == id).FirstOrDefault().UserId;

            return result;
        }

    
    }
}
