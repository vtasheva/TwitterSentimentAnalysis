using CoreTweet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwitterSentimentAnalysis.Models;

namespace TwitterSentimentAnalysis.Managers
{
    public class TwitterManager
    {
        private readonly OAuth2Token _token;

        public TwitterManager()
        {
            _token = OAuth2.GetToken("AhgErASKehSwA3Ktsaw1A", "GqerOT4WN9PnrWaHhz3U8Vqr28Lp3ZGPTzRb9JSL8Lg");
        }

        public IEnumerable<TweetModel> SearchTweets(string searchText, int tweetCount = 5)
        {
            var tweets = _token.Search.Tweets(searchText, lang: "en", count: tweetCount, tweet_mode: TweetMode.Extended);
            var data = tweets.Select(x => 
                new TweetModel
                {
                    Id = x.Id.ToString(),
                    Text = x.RetweetedStatus == null ? x.FullText : x.RetweetedStatus.FullText,
                });

            return data;
        }
    }
}