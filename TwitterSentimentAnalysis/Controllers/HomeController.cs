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

        public HomeController()
        {
            _twitterManager = new TwitterManager();
            _monkeyLearnManager = new MonkeyLearnManager();
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

            return PartialView("_SearchResults", classifiedData);
        }
    }
}