using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwitterSentimentAnalysis.Models;

namespace TwitterSentimentAnalysis.Managers
{
    public class MonkeyLearnManager
    {
        private readonly string AuthorizationToken = "7e1dc21eca52c53ddc0156447b279822b7f9486b";
        private readonly string ClassifierUri = "https://api.monkeylearn.com/v3/classifiers/cl_4qNvmLar/classify/";

        public async Task<IEnumerable<ClassifiedTweetModel>> Classify(IEnumerable<TweetModel> tweets)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, ClassifierUri))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Token {AuthorizationToken}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var data = JsonConvert.SerializeObject(new { data = tweets });
                using (var stringContent = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    var response = await client.SendAsync(request);
                    var values = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<ClassifiedTweetModel>>(values);

                }
            }

        }
    }
}