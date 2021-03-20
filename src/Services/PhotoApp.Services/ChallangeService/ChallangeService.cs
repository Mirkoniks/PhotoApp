using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Services.Models.Challange;
using PhotoApp.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using PhotoApp.Services.PhotoService;
using PhotoApp.Services.CloudinaryService;
using CloudinaryDotNet;
using PhotoApp.Services.Models.Photo;

namespace PhotoApp.Services.ChallangeService
{
    public class ChallangeService : IChallangeService
    {

        public static DateTime TodayDate { get; set; }

        private readonly PhotoAppDbContext dbContext;
        private readonly IPhotoService photoService;
        private readonly ICloundinaryService cloundinaryService;
        private readonly Cloudinary cloudinary;

        public ChallangeService(PhotoAppDbContext dbContext, IPhotoService photoService, ICloundinaryService cloundinaryService, Cloudinary cloudinary)
        {
            this.dbContext = dbContext;
            this.photoService = photoService;
            this.cloundinaryService = cloundinaryService;
            this.cloudinary = cloudinary;
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


            await dbContext.Challanges.AddAsync(challange);
            await dbContext.SaveChangesAsync();

            int challangeId = challange.ChallangeId;

            await RunChallageCheckAsync(challangeId);

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
                IsUpcoming = challange.IsUpcoming,
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
            var photos = dbContext.PhotosChallanges.Where(c => c.ChallangeId == challangeId).OrderByDescending(v => v.VotesCount).Take(numPhotos).ToList();

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
                photoServiceModel.ChallangeName = await GetChallangeNameById(photo.ChallangeId);

                photoServiceModels.Add(photoServiceModel);
            }

            serviceModel.Photos = photoServiceModels;

            return serviceModel;
        }

        public async Task<TopPhotosServiceModel> GetLatestPhotos(int numPhotos)
        {
            var photos = dbContext.PhotosChallanges.OrderByDescending(v => v.PhotoId).Take(numPhotos).ToList();

            TopPhotosServiceModel serviceModel = new TopPhotosServiceModel();
            List<TopPhotoServiceModel> photoServiceModels = new List<TopPhotoServiceModel>();

            foreach (var photo in photos)
            {
                TopPhotoServiceModel photoServiceModel = new TopPhotoServiceModel();

                photoServiceModel.VotesCount = photo.VotesCount;
                photoServiceModel.PhotoLink = GetPhotoLinkById(photo.PhotoId);
                photoServiceModel.UserId = GetUserIdByPicureId(photo.PhotoId);
                photoServiceModel.ChallangeName = await GetChallangeNameById(photo.ChallangeId);

                photoServiceModels.Add(photoServiceModel);
            }

            serviceModel.Photos = photoServiceModels;

            return serviceModel;
        }

        public async Task<string> GetChallangeNameById(int id)
        {
            var challange = dbContext.Challanges.Where(c => c.ChallangeId == id).FirstOrDefault().Name;

            return challange;
        }

        public async Task<TopPhotosServiceModel> GetTopPhotosFromChallange(int challangeId, int numPhotos, int startPhotoId = 0)
        {
            throw new NotImplementedException();
        }

        public async Task RunChallagesCheckAsync()
        {
            var challanges = dbContext.Challanges.ToList();

            for (int i = 0; i < challanges.Count; i++)
            {

                switch (CheckChallangeStatus(challanges[i].StartTime, challanges[i].EndTime))
                {
                    case -1:
                        var challange = dbContext.Challanges.Where(c => c.ChallangeId == challanges[i].ChallangeId).FirstOrDefault().IsUpcoming = true;
                        break;
                    case 0:
                        var challange1 = dbContext.Challanges.Where(c => c.ChallangeId == challanges[i].ChallangeId).FirstOrDefault().IsOpen = true;
                        break;
                    case 1:
                        var challange2 = dbContext.Challanges.Where(c => c.ChallangeId == challanges[i].ChallangeId).FirstOrDefault().IsOpen = false;
                        break;
                    default:
                        break;
                }
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task RunChallageCheckAsync(int id)
        {
            var challangeDb = dbContext.Challanges.Where(c => c.ChallangeId == id).FirstOrDefault(); ;

            switch (CheckChallangeStatus(challangeDb.StartTime, challangeDb.EndTime))
            {
                case -1:
                    var challange = dbContext.Challanges.Where(c => c.ChallangeId == challangeDb.ChallangeId).FirstOrDefault().IsUpcoming = true;
                    dbContext.Challanges.Where(c => c.ChallangeId == challangeDb.ChallangeId).FirstOrDefault().IsOpen = false;
                    break;
                case 0:
                    var challange1 = dbContext.Challanges.Where(c => c.ChallangeId == challangeDb.ChallangeId).FirstOrDefault().IsOpen = true;
                    dbContext.Challanges.Where(c => c.ChallangeId == challangeDb.ChallangeId).FirstOrDefault().IsUpcoming = false;
                    break;
                case 1:
                    var challange2 = dbContext.Challanges.Where(c => c.ChallangeId == challangeDb.ChallangeId).FirstOrDefault().IsOpen = false;
                    dbContext.Challanges.Where(c => c.ChallangeId == challangeDb.ChallangeId).FirstOrDefault().IsUpcoming = false;
                    break;
                default:
                    break;
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task SetChallangeCoverPhoto(int challangeId, int photoId)
        {
            dbContext.Challanges.Where(c => c.ChallangeId == challangeId).FirstOrDefault().ChallangeCoverPhotoId = photoId;

            await dbContext.SaveChangesAsync();
        }



        //temporary  solution
        //if -1 is upcommig, if 0 is now, if 1 closed
        private int CheckChallangeStatus(DateTime startTime, DateTime endTime)
        {
            DateTime now = DateTime.UtcNow.Date.AddDays(-1);

            if ((DateTime.Compare(now, startTime) == -1) && (DateTime.Compare(now, endTime) == -1))
            {
                return -1;
            }
            else if ((DateTime.Compare(now, startTime) == 0) && (DateTime.Compare(now, endTime) == -1))
            {
                return 0;
            }
            else if ((DateTime.Compare(now, startTime) == 1) && (DateTime.Compare(now, endTime) == -1))
            {
                return 0;
            }
            else if ((DateTime.Compare(now, startTime) == 1) && (DateTime.Compare(now, endTime) == 0))
            {
                return 0;
            }

            return 1;
        }

        public async Task<AllChallangesServiceModel> GetAllOpenChallanges()
        {
            IEnumerable<Challange> challanges = dbContext.Challanges.Where(c => c.IsOpen == true).ToList();

            AllChallangesServiceModel serviceModel = new AllChallangesServiceModel();
            List<ChallangeServiceModel> challangesList = new List<ChallangeServiceModel>();

            foreach (var item in challanges)
            {
                var photo = await photoService.FindPhotoByIdAsync(item.ChallangeCoverPhotoId);
                ChallangeServiceModel challange = new ChallangeServiceModel
                {
                    Id = item.ChallangeId,
                    Name = item.Name,
                    Description = item.Description,
                    IsOpen = item.IsOpen,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    ChallangeCoverPhotoLink = photo.PhotoLink
                };
                challangesList.Add(challange);
            }

            serviceModel.Challanges = challangesList;

            return serviceModel;
        }

        public async Task<AllChallangesServiceModel> GetAllUpcomingChallanges()
        {
            IEnumerable<Challange> challanges = dbContext.Challanges.Where(c => c.IsUpcoming == true).ToList();

            AllChallangesServiceModel serviceModel = new AllChallangesServiceModel();
            List<ChallangeServiceModel> challangesList = new List<ChallangeServiceModel>();

            foreach (var item in challanges)
            {
                var photo = await photoService.FindPhotoByIdAsync(item.ChallangeCoverPhotoId);
                ChallangeServiceModel challange = new ChallangeServiceModel
                {
                    Id = item.ChallangeId,
                    Name = item.Name,
                    Description = item.Description,
                    IsOpen = item.IsOpen,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    ChallangeCoverPhotoLink = photo.PhotoLink
                };

                challangesList.Add(challange);
            }

            serviceModel.Challanges = challangesList;

            return serviceModel;
        }
        public async Task<AllChallangesServiceModel> GetAllClosedCallanges()
        {
            IEnumerable<Challange> challanges = dbContext.Challanges.Where(c => c.IsOpen == false).Where(c => c.IsUpcoming == false).ToList();

            AllChallangesServiceModel serviceModel = new AllChallangesServiceModel();
            List<ChallangeServiceModel> challangesList = new List<ChallangeServiceModel>();

            foreach (var item in challanges)
            {
                var photo = await photoService.FindPhotoByIdAsync(item.ChallangeCoverPhotoId);
                ChallangeServiceModel challange = new ChallangeServiceModel
                {
                    Id = item.ChallangeId,
                    Name = item.Name,
                    Description = item.Description,
                    IsOpen = item.IsOpen,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    ChallangeCoverPhotoLink = photo.PhotoLink
                };
                challangesList.Add(challange);
            }

            serviceModel.Challanges = challangesList;

            return serviceModel;
        }
        public async Task EditChallange(EditChallangeServiceModel editChallangeServiceModel)
        {
            var challange = dbContext.Challanges.Where(c => c.ChallangeId == editChallangeServiceModel.ChallangeId).FirstOrDefault();

            if (editChallangeServiceModel.ChallangeCoverPhoto != null)
            {
                var photoLink = await cloundinaryService.UploadAsync(cloudinary, editChallangeServiceModel.ChallangeCoverPhoto);

                var photoId = await photoService.AddPhotoAsync(photoLink.FirstOrDefault());

                challange.ChallangeCoverPhotoId = photoId;
            }

            challange.Name = editChallangeServiceModel.Name;
            challange.Description = editChallangeServiceModel.Description;
            challange.StartTime = editChallangeServiceModel.StartTime;
            challange.EndTime = editChallangeServiceModel.EndTime;

            await dbContext.SaveChangesAsync();

            await RunChallageCheckAsync(editChallangeServiceModel.ChallangeId);
        }

        public async Task<UserLikedPhotosServiceModel> GetUserLikedPhotos(string userId)
        {
            var photos = dbContext.UsersPhotoLikes.Where(u => u.UserId == userId).ToList();
            UserLikedPhotosServiceModel userLikedPhotosServiceModel = new UserLikedPhotosServiceModel();

            if (photos != null)
            {
                List<UserLikedPhotoServiceModel> list = new List<UserLikedPhotoServiceModel>();

                foreach (var item in photos)
                {
                    var photoLink = await photoService.FindPhotoByIdAsync(item.PhotoId);

                    UserLikedPhotoServiceModel userLikedPhotoServiceModel = new UserLikedPhotoServiceModel()
                    {
                        PhotoLink = photoLink.PhotoLink
                    };


                    list.Add(userLikedPhotoServiceModel);

                    userLikedPhotosServiceModel.Photos = list;
                }
            }

            return userLikedPhotosServiceModel;
        }

        public async Task<UserPhotosServiceModel> GetUserPhotos(string userId)
        {
            var photos = dbContext.UsersPhotos.Where(u => u.UserId == userId).ToList();

            UserPhotosServiceModel userPhotosServiceModel = new UserPhotosServiceModel();
            List<UserPhotoServiceModel> list = new List<UserPhotoServiceModel>();

            if (photos != null)
            {
                foreach (var item in photos)
                {
                    var photo = await photoService.FindPhotoByIdAsync(item.PhotoId);

                    UserPhotoServiceModel serviceModel = new UserPhotoServiceModel
                    {
                        PhotoLink = photo.PhotoLink
                    };

                    list.Add(serviceModel);
                }
            }
            userPhotosServiceModel.Photo = list;

            return userPhotosServiceModel;
        }


        public async Task DeleteChallange(int id)
        {
            dbContext.Challanges.Where(c => c.ChallangeId == id).FirstOrDefault().IsDeleted = true;

            await dbContext.SaveChangesAsync();
        }

        public async Task<int> GetUpcomigChallangesCount()
        {
            int count = dbContext.Challanges.Where(c => c.IsUpcoming == true).Where(c => c.IsOpen == false).Count();

            return count;
        }

        public async Task<int> GetOpenChallangesCount()
        {
            int count = dbContext.Challanges.Where(c => c.IsOpen == true).Where(c => c.IsUpcoming == false).Count();

            return count;
        }

        public async Task<int> GetClosedChallangesCount()
        {
            int count = dbContext.Challanges.Where(c => c.IsOpen == false).Where(c => c.IsUpcoming == false).Count();

            return count;
        }

        public async Task<AdminAllChallangesServiceModel> AdminGetAllChallanges()
        {
            AdminAllChallangesServiceModel adminAllChallangesServiceModel = new AdminAllChallangesServiceModel();
            List<AdminChallangeServiceModel> adminChallangeServiceModels = new List<AdminChallangeServiceModel>();

            var challangesDb = dbContext.Challanges.Take(dbContext.Challanges.Count()).ToList();

            foreach (var item in challangesDb)
            {
                AdminChallangeServiceModel model = new AdminChallangeServiceModel
                {
                    Id = item.ChallangeId,
                    Name = item.Name,
                    Description = item.Description,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    IsOpen = item.IsOpen,
                    IsUpcoming = item.IsUpcoming
                };

                adminChallangeServiceModels.Add(model);
            }


            adminAllChallangesServiceModel.Challanges = adminChallangeServiceModels;

            return adminAllChallangesServiceModel;
        }

        public async Task<AdminAllChallangesServiceModel> AdminGetAllOpenChallanges()
        {
            AdminAllChallangesServiceModel adminAllChallangesServiceModel = new AdminAllChallangesServiceModel();
            List<AdminChallangeServiceModel> adminChallangeServiceModels = new List<AdminChallangeServiceModel>();

            var challangesDb = dbContext.Challanges.Where(c => c.IsOpen == true).Take(dbContext.Challanges.Where(c => c.IsOpen == true).Count());

            foreach (var item in challangesDb)
            {
                AdminChallangeServiceModel model = new AdminChallangeServiceModel
                {
                    Id = item.ChallangeId,
                    Name = item.Name,
                    Description = item.Description,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    IsOpen = item.IsOpen,
                    IsUpcoming = item.IsUpcoming
                };

                adminChallangeServiceModels.Add(model);
            }


            adminAllChallangesServiceModel.Challanges = adminChallangeServiceModels;

            return adminAllChallangesServiceModel;
        }

        public async Task<AdminAllChallangesServiceModel> AdminGetAllUpcomingChallanges()
        {
            AdminAllChallangesServiceModel adminAllChallangesServiceModel = new AdminAllChallangesServiceModel();
            List<AdminChallangeServiceModel> adminChallangeServiceModels = new List<AdminChallangeServiceModel>();

            var challangesDb = dbContext.Challanges.Where(c => c.IsUpcoming == true).Take(dbContext.Challanges.Where(c => c.IsUpcoming == true).Count());

            foreach (var item in challangesDb)
            {
                AdminChallangeServiceModel model = new AdminChallangeServiceModel
                {
                    Id = item.ChallangeId,
                    Name = item.Name,
                    Description = item.Description,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    IsOpen = item.IsOpen,
                    IsUpcoming = item.IsUpcoming
                };

                adminChallangeServiceModels.Add(model);
            }


            adminAllChallangesServiceModel.Challanges = adminChallangeServiceModels;

            return adminAllChallangesServiceModel;
        }

        public async Task<AdminAllChallangesServiceModel> AdminGetAllClosedChallanges()
        {
            AdminAllChallangesServiceModel adminAllChallangesServiceModel = new AdminAllChallangesServiceModel();
            List<AdminChallangeServiceModel> adminChallangeServiceModels = new List<AdminChallangeServiceModel>();

            var challangesDb = dbContext.Challanges.Where(c => c.IsOpen == false)
                                                   .Where(c => c.IsUpcoming == false)
                                                   .Take(dbContext.Challanges
                                                   .Where(c => c.IsOpen == false)
                                                   .Where(c => c.IsUpcoming == false)
                                                   .Count());

            foreach (var item in challangesDb)
            {
                AdminChallangeServiceModel model = new AdminChallangeServiceModel
                {
                    Id = item.ChallangeId,
                    Name = item.Name,
                    Description = item.Description,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    IsOpen = item.IsOpen,
                    IsUpcoming = item.IsUpcoming
                };

                adminChallangeServiceModels.Add(model);
            }


            adminAllChallangesServiceModel.Challanges = adminChallangeServiceModels;

            return adminAllChallangesServiceModel;

        }

        public async Task<string> SetStatus(bool isOpen, bool isUpcoming)
        {
            if (isOpen == true && isUpcoming == false)
            {
                return "Ongoing";
            }
            else if (isOpen == false && isUpcoming == false)
            {
                return "Closed";
            }

            return "Upcoming";
        }

        public async Task<AdminChallangeServiceModel> GetChallangeById(int id)
        {
            var challangeDb = dbContext.Challanges.Where(c => c.ChallangeId == id).FirstOrDefault();

            AdminChallangeServiceModel adminChallangeServiceModel = new AdminChallangeServiceModel()
            {
                Id = challangeDb.ChallangeId,
                Name = challangeDb.Name,
                Description = challangeDb.Description,
                StartTime = challangeDb.StartTime,
                EndTime = challangeDb.EndTime,
                IsOpen = challangeDb.IsOpen,
                IsUpcoming = challangeDb.IsUpcoming
            };

            return adminChallangeServiceModel;
        }


        private string GetPhotoLinkById(int id)
        {
            return dbContext.Photos.Where(p => p.PhotoId == id).FirstOrDefault().Link;
        }

        private string GetUserIdByPicureId(int id)
        {
            var result = dbContext.UsersPhotos.Where(p => p.PhotoId == id).FirstOrDefault().UserId;

            return result;
        }

        public async Task<bool> IsValidId(int id)
        {
            if (id < 0)
            {
                return false;
            }

            var bol = dbContext.Challanges.Any(c => c.ChallangeId == id);

            return bol;
        }
    }
}