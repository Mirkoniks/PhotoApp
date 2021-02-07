using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class LoadInfo
    {
        [JsonProperty("photosSend")]
        public int PhotosSend { get; set; }

        [JsonProperty("totalPhotos")]
        public int TotalPhotos { get; set; }

        [JsonProperty("challangeId")]
        public int ChallangeId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
