using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class TopPhotosLoadInfo
    {
        [JsonProperty("photosSent")]
        public int PhotosSent { get; set; }
    }
}
