using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class PhotoViewModel
    {
        [JsonProperty("photoId")]
        public int PhotoId { get; set; }

        [JsonProperty("photoLink")]
        public string PhotoLink { get; set; }
    }
}
