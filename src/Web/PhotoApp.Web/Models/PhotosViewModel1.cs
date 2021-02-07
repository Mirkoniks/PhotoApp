using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class PhotosViewModel1
    {
        [JsonProperty("photos")]
        public IEnumerable<PhotoViewModel> Photos { get; set; }

        [JsonProperty("photosCount")]
        public int PhotosCount { get; set; }

        [JsonProperty("totalPhotos")]
        public int TotalPhotos { get; set; }

        [JsonProperty("stepsCount")]
        public int StepsCount { get; set; }

        [JsonProperty("isOpen")]
        public bool IsOpen { get; set; }

        [JsonProperty("isUpcoming")]
        public bool IsUpcomig { get; set; }
    }
}
