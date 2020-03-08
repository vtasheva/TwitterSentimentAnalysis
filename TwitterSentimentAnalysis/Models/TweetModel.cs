using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterSentimentAnalysis.Models
{
    public class TweetModel
    {
        [JsonProperty("external_id")]
        public string Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}