using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class MessageModel
    {
        [JsonProperty("toUserId")]
        public string ToUserId { get; set; }

        [JsonProperty("fromUserId")]
        public string FromUserId { get; set; }

        [JsonProperty("namesFrom")]
        public string NamesFrom { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
