using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReviewAnalyzerAPI.Models
{
    public class SentimentAnalysisResponse
    {
        public List<SentimentPrediction> SentimentPrediction { get; set; }
    }
}