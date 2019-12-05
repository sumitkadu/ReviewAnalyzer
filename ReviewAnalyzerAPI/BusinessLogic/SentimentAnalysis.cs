using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using static Microsoft.ML.DataOperationsCatalog;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms.Text;
using ReviewAnalyzerAPI.Models;

namespace ReviewAnalyzerAPI
{
    public class SentimentAnalysis
    {
        static readonly string _dataPath = System.Configuration.ConfigurationManager.AppSettings["TrainingDataFilePath"].ToString();


        public TrainTestData LoadData(MLContext mlContext)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<SentimentData>(_dataPath, hasHeader: false);
            TrainTestData splitDataView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            return splitDataView;
        }

        public ITransformer BuildAndTrainModel(MLContext mlContext, IDataView splitTrainSet)
        {
            var estimator = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(SentimentData.SentimentText))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));
            //Console.WriteLine("=============== Create and Train the Model ===============");
            var model = estimator.Fit(splitTrainSet);
            //Console.WriteLine("=============== End of training ===============");
            //Console.WriteLine();
            return model;
        }

        public CalibratedBinaryClassificationMetrics Evaluate(MLContext mlContext, ITransformer model, IDataView splitTestSet)
        {
            //Console.WriteLine("=============== Evaluating Model accuracy with Test data===============");
            IDataView predictions = model.Transform(splitTestSet);
            return mlContext.BinaryClassification.Evaluate(predictions, "Label");
            //Console.WriteLine();
            //Console.WriteLine("Model quality metrics evaluation");
            //Console.WriteLine("--------------------------------");
            //Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            //Console.WriteLine($"Auc: {metrics.AreaUnderRocCurve:P2}");
            //Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
            //Console.WriteLine("=============== End of model evaluation ===============");
        }
        public SentimentAnalysisResponse PredictSentiments(MLContext mlContext, ITransformer model, List<SentimentData> sentiments)
        {           
            IDataView batchComments = mlContext.Data.LoadFromEnumerable(sentiments);
            IDataView predictions = model.Transform(batchComments);

            // Use model to predict whether comment data is Positive (1) or Negative (0).
            IEnumerable<SentimentPrediction> predictedResults = mlContext.Data.CreateEnumerable<SentimentPrediction>(predictions, reuseRowObject: false);
            //Console.WriteLine();

            SentimentAnalysisResponse response = new SentimentAnalysisResponse();
            List<SentimentPrediction> sentimentPrediction = new List<SentimentPrediction>();
            foreach (SentimentPrediction prediction in predictedResults)
            {
                sentimentPrediction.Add(new SentimentPrediction() { Prediction = prediction.Prediction, Probability = prediction.Probability, Score = prediction.Score });
            }

            response.SentimentPrediction = sentimentPrediction;
            return response;
        }
    }
}
