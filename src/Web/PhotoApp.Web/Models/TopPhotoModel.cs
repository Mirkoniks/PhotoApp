using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class TopPhotoModel
    {
        [JsonProperty("photoLink")]
        public string PhotoLink { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("votesCount")]
        public int VotesCount { get; set; }
    }
}
