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
    }
}