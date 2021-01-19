﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Models
{
    public class LoadInfo
    {
        [JsonProperty("stepsCount")]
        public int StepsCount { get; set; }

        [JsonProperty("totalPhotos")]
        public int TotalPhotos { get; set; }

        [JsonProperty("challangeId")]
        public int ChallangeId { get; set; }
    }
}
