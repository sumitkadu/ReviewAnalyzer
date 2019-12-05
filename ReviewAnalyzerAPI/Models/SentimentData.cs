using Microsoft.ML.Data;
using System.Runtime.Serialization;

namespace ReviewAnalyzerAPI.Models
{
    public class SentimentData
    {
        [LoadColumn(0)]        
        public string SentimentText;

        [LoadColumn(1), ColumnName("Label")]
        public bool Sentiment;
    }

    [DataContract]
    public class SentimentPrediction : SentimentData
    {
        [DataMember]
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        [DataMember]
        public float Probability { get; set; }

        public float Score { get; set; }
    }
}
