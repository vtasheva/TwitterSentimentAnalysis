using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwitterSentimentAnalysis.Models
{
    public class SearchModel
    {
        [Required]
        [Display(Name = "Search text")]
        public string SearchText { get; set; }
        [Required]
        [Display(Name = "Tweet count")]
        public int TweetCount { get; set; }
    }
}