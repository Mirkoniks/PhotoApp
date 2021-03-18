using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class LatestPhotosModel
    {
        [JsonProperty("photos")]
        public IEnumerable<LatestPhotoModel> Photos { get; set; }

        [JsonProperty("expectMorePhotos")]
        public bool ExpectMorePhotos { get; set; }
    }
}
