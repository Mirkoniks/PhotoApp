using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class Like
    {
        [JsonProperty("photoId")]
        public int PhotoId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("challangeId")]
        public int ChallangeId { get; set; }
    }
}
