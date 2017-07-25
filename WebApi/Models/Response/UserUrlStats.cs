using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class UserUrlStats
    {
        [JsonProperty("userid")]
        public string userid { get; set; }

        public UrlStats UrlUserStats { get; set; }

        public UserUrlStats()
        {
            UrlUserStats = new UrlStats();
        }
    }
}