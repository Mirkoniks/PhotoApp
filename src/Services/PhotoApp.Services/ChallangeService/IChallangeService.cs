﻿using PhotoApp.Services.Models.Challange;
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
    }
}
