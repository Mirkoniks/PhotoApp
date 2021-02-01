using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class TopPhotosModel
    {
        [JsonProperty("photos")]
        public IEnumerable<TopPhotoModel> Photos { get; set; }

        [JsonProperty("photosSend")]
        public int PhotosSend { get; set; }

        [JsonProperty("challangeId")]
        public int ChallangeId { get; set; }
    }
}
