using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class PhotosViewModel
    {
        [JsonProperty("photosSent")]
        public int PhotosSent { get; set; }

        [JsonProperty("photos")]
        public IEnumerable<PhotoViewModel> Photos { get; set; }

        [JsonProperty("expectMorePhotos")]
        public bool ExpectMorePhotos { get; set; }

        [JsonProperty("challangeId")]
        public int ChallangeId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("actualPhotosCount")]
        public int ActualPhotosCount { get; set; }
    }
}
