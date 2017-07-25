using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteEnkurtUrl.Models.Response
{
    public class Url
    {
        [JsonProperty("url")]
        public string RealUrl { get; set; }

        [JsonProperty("shortUrl")]
        public string ShortenedUrl { get; set; }

        [JsonProperty("userid",  NullValueHandling=NullValueHandling.Ignore)]
        public string CreatedBy { get; set; }

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("hits")]
        public int hits { get; set; }
    }
}