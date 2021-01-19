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

        [JsonProperty("stepsCount")]
        public int StepsCount { get; set; }

    }
}
