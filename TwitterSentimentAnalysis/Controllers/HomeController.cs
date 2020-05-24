using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwitterSentimentAnalysis.Managers;
using TwitterSentimentAnalysis.Models;

namespace TwitterSentimentAnalysis.Controllers
{
    public class HomeController : Controller
    {
        private readonly TwitterManager _twitterManager;
        private readonly MonkeyLearnManager _monkeyLearnManager;
        private readonly SlangManager _slangManager;

        public HomeController()
        {
            _twitterManager = new TwitterManager();
            _monkeyLearnManager = new MonkeyLearnManager();
            _slangManager = new SlangManager();
        }

        // GET: Training
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(SearchModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var searchText = model.SearchText;
            var data = _twitterManager.SearchTweets(searchText, model.TweetCount);

            var classifiedData = await _monkeyLearnManager.Classify(data);
            classifiedData = _slangManager.ModifyConfidence(classifiedData);

            var searchResultsModel = new SearchResultsModel
            {
                RowsCount = GetTotalNumberOfRows(classifiedData),
                PositiveTweets = GetTweetsByTagName(classifiedData, "Positive"),
                NegativeTweets = GetTweetsByTagName(classifiedData, "Negative"),
                NeutralTweets = GetTweetsByTagName(classifiedData, "Neutral"),
            };

            var totalNumberOfTweets = (double)searchResultsModel.PositiveTweets.Count() + searchResultsModel.NegativeTweets.Count() + searchResultsModel.NeutralTweets.Count();

            var dataPoints = new List<DataPointModel>
            {
                new DataPointModel { Label = "Positive", Y = (searchResultsModel.PositiveTweets.Count() / totalNumberOfTweets) * 100 },
                new DataPointModel { Label = "Negative", Y = (searchResultsModel.NegativeTweets.Count() / totalNumberOfTweets) * 100 },
                new DataPointModel { Label = "Neutral", Y = (searchResultsModel.NeutralTweets.Count() / totalNumberOfTweets) * 100 },
            };

            searchResultsModel.ChartData = JsonConvert.SerializeObject(dataPoints);

            return PartialView("_SearchResults", searchResultsModel);
        }

        private ClassifiedTweetModel[] GetTweetsByTagName(IEnumerable<ClassifiedTweetModel> data, string tagName)
        {
            return data.Where(x => x.Classifications.Any() && x.Classifications.OrderByDescending(t => t.Confidence).First().TagName == tagName).ToArray();
        }

        private int GetTotalNumberOfRows(IEnumerable<ClassifiedTweetModel> data)
        {
            return data.Where(x => x.Classifications.Any() && x.Classifications.OrderByDescending(t => t.Confidence).First().TagName != "Not classified")
                .GroupBy(x => x.Classifications.OrderByDescending(t => t.Confidence).First().TagName).Select(g => g.Count()).Max();
        }

        private IEnumerable<TweetModel> GetTestData()
        {
            var data = new List<TweetModel>
            {
                new TweetModel { Id = "1", Text = "Soccer now sounds like tennis. #bunsesliga #footballisback" },
                new TweetModel { Id = "2", Text = "Roland Garros (the French Open tennis) was due to start today. In its absence, Roland Garros Re-Lived will start today, from The @TennisPodcast." },
                new TweetModel { Id = "3", Text = "@JBaker31826004 @realDonaldTrump @FLOTUS Hardly! The more she speaks about #bebest and builds tennis courts, the more she disgusts me." },
                new TweetModel { Id = "4", Text = "@Topspin_righty You are right but after that he had problems with the knee. I don't if there is a correlation but before the surgery he was supposed to play only RG on clay." },
                new TweetModel { Id = "5", Text = "i can’t believe they made the flowers in animal crossing in real life 😍😍😍 https://t.co/Bw2e3OO60q" },
                new TweetModel { Id = "6", Text = "renjun : although in reality, not all dreams will bloom into flowers and not everyone’s dreams will come true. but every dream is wonderful, everyone’s life is much more exciting as they chase after their dreams." },
                new TweetModel { Id = "7", Text = "Mad how Rodgers wasn’t sacked that night.... we can only assume the club only wanted klopp and that it was more important to bide our time I always find in interesting that we signed Milner and bobby that summer which were perfect for klopp https://t.co/axOy13mtvB" },
                new TweetModel { Id = "8", Text = "The Jersey Shore: We need it, we want it, we can't stay away from it. Here's my appreciation of the state's greatest asset, and why this summer will be the strangest, most special one ever at the shore https://t.co/agN9Lm04gQ" },
                new TweetModel { Id = "9", Text = "The MORE TIME you spend and the CLOSER IN SPACE you are to any infected people, the higher your risk. Interacting with MORE PEOPLE raises your risk, and INDOOR places are riskier than outdoors." },
                new TweetModel { Id = "10", Text = "@DannyGradio Nah I gotcha, GRadio! I was just adding my own Flava to it! I’ve seen a thousand of these memes, I merely meant I’d rather spend the summer with Tonya, there would be lots of beer drinking, a lotta “Wrastlin’” and good times! Nancy looks looks boring as hell, too prissy for me!!!" },
            };

            return data;
        }
    }
}