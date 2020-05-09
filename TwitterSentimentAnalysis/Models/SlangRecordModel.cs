using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterSentimentAnalysis.Models
{
    public class SlangRecordModel
    {
        public string Text { get; set; }
        public int SentimentScore { get; set; }
    }
}