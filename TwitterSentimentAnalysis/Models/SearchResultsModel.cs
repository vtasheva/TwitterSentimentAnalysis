using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterSentimentAnalysis.Models
{
    public class SearchResultsModel
    {
        public int RowsCount { get; set; }
        public ClassifiedTweetModel[] PositiveTweets { get; set; }
        public ClassifiedTweetModel[] NegativeTweets { get; set; }
        public ClassifiedTweetModel[] NeutralTweets { get; set; }
        public string ChartData { get; set; }
    }
}