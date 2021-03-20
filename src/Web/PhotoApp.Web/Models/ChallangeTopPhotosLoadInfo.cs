using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class ChallangeTopPhotosLoadInfo
    {
        [JsonProperty("photosSent")]
        public int PhotosSent { get; set; }

        [JsonProperty("challangeId")]
        public int ChallangeId { get; set; }
    }
}
