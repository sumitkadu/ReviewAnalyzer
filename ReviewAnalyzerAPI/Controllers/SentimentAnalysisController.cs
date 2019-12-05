using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.ML;
using Microsoft.ML.Data;
using static Microsoft.ML.DataOperationsCatalog;
using ReviewAnalyzerAPI.Models;

namespace ReviewAnalyzerAPI.Controllers
{
    public class SentimentAnalysisController : ApiController
    {
        public SentimentAnalysisResponse Post(List<string> sentimentData)
        {
            List<SentimentData> sentimentDatas = new List<SentimentData>();
            foreach(string s in sentimentData)
            {
                sentimentDatas.Add(new SentimentData() { SentimentText = s });
            }

            SentimentAnalysis sentimentAnalysis = new SentimentAnalysis();
            MLContext mlContext = new MLContext();
            TrainTestData splitDataView = sentimentAnalysis.LoadData(mlContext);
            ITransformer model = sentimentAnalysis.BuildAndTrainModel(mlContext, splitDataView.TrainSet);
            sentimentAnalysis.Evaluate(mlContext, model, splitDataView.TestSet);
            //UseModelWithSingleItem(mlContext, model);
            return sentimentAnalysis.PredictSentiments(mlContext, model, sentimentDatas);
        }
    }
}
