using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class UrlStats
    {
        [JsonProperty("hits", NullValueHandling = NullValueHandling.Ignore)]
        public int? hits { get; set; }

        [JsonProperty("urlCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? urlCount { get; set; }

        [JsonProperty("topUrls", NullValueHandling = NullValueHandling.Ignore)]
        public List<Url> topUrls { get; set; }
    }
}