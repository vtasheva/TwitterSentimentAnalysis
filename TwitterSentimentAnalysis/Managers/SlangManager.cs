using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TwitterSentimentAnalysis.Models;

namespace TwitterSentimentAnalysis.Managers
{
    public class SlangManager
    {
        private List<SlangRecordModel> slangRecords;

        public SlangManager()
        {
            InitializeSlangRecords();
        }

        public IEnumerable<ClassifiedTweetModel> ModifyConfidence(IEnumerable<ClassifiedTweetModel> dataItems)
        {
            foreach (var item in dataItems)
            {
                var positiveConfidence = 0d;
                var negativeConfidence = 0d;

                var words = item.Text.Split().Where(x => !x.StartsWith("@"));
                words = words.Select(x => Regex.Replace(x, @"[^\w]", ""));
                words = words.Where(x => !string.IsNullOrWhiteSpace(x));
                var wordsCount = words.Count();
                for (int chunkSize = 1; chunkSize <= wordsCount; chunkSize++)
                {
                    var chunks = Enumerable.Range(0, wordsCount - chunkSize + 1)
                            .Select(i => string.Join(" ", words.Skip(i).Take(chunkSize)));

                    foreach (var chunk in chunks)
                    {
                        if (slangRecords.Any(x => x.Text == chunk))
                        {
                            var record = slangRecords.First(x => x.Text == chunk);
                            if (record.SentimentScore < 0)
                            {
                                negativeConfidence += 0.1 * Math.Abs(record.SentimentScore);
                            }

                            if (record.SentimentScore > 0)
                            {
                                positiveConfidence += 0.1 * Math.Abs(record.SentimentScore);
                            }
                        }
                    }
                }

                if (positiveConfidence > 0)
                {
                    AddConfidence(item, "Positive", positiveConfidence);
                }

                if (negativeConfidence > 0)
                {
                    AddConfidence(item, "Negative", negativeConfidence);
                }
            }

            return dataItems;
        }

        private static void AddConfidence(ClassifiedTweetModel item, string tagName, double confidence)
        {
            if (item.Classifications.Any(x => x.TagName == tagName))
            {
                var tag = item.Classifications.First(x => x.TagName == tagName);
                tag.Confidence += confidence;
            }
            else
            {
                item.Classifications.Add(new ClassificationModel { TagName = tagName, Confidence = confidence });
            }
        }

        private void InitializeSlangRecords()
        {
            var lines = File.ReadAllLines(HttpContext.Current.Server.MapPath("~/SlangSD/SlangSD.txt"));
            slangRecords = new List<SlangRecordModel>();

            foreach (var line in lines)
            {
                var record = line.Split('\t');
                var model = new SlangRecordModel
                {
                    Text = record[0],
                    SentimentScore = int.Parse(record[1]),
                };

                slangRecords.Add(model);
            }
        }
    }
}