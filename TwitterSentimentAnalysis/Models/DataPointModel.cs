using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterSentimentAnalysis.Models
{
    public class DataPointModel
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("y")]
        public double Y { get; set; }
    }
}