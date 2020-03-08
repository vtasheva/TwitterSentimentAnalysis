using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterSentimentAnalysis.Models
{
    public class ClassifiedTweetModel
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public bool Error { get; set; }
        public IEnumerable<ClassificationModel> Classifications { get; set; }
    }
}