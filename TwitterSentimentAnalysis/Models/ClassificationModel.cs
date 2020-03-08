using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterSentimentAnalysis.Models
{
    public class ClassificationModel
    {
        [JsonProperty("tag_id")]
        public long TagId { get; set; }
        [JsonProperty("tag_name")]
        public string TagName { get; set; }
        [JsonProperty("confidence")]
        public double Confidence { get; set; }
    }
}