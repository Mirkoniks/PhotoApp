using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class ChallangeTopPhotosModel
    {
        [JsonProperty("photos")]
        public IEnumerable<ChallangeTopPhotoModel> Photos { get; set; }

        [JsonProperty("expectMorePhotos")]
        public bool ExpectMorePhotos { get; set; }
    }
}
