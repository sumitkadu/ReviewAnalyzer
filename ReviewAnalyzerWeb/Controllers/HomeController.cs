using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Review_Analyzer.Models;

namespace Review_Analyzer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadFile(HttpPostedFileBase uploadFile)
        {
            ViewBag.Message = "Your application description page.";
            ReviewModel model = new ReviewModel();
            try
            {
                if (uploadFile.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(uploadFile.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    uploadFile.SaveAs(_path);
                    List<string> fileText = System.IO.File.ReadAllLines(_path).ToList();
                    var result = MakeRequest(fileText);
                    int total = result.Count;
                    int posstive = result.Count(x => x.Prediction.Value);
                    int negative = result.Count(x => !x.Prediction.Value);
                    model.NegativeSentiments = (negative * 100 / total) + "%";
                    model.PositiveSentiments = (posstive * 100 / total) + "%";
                    model.ShowSentiments = true;
                }
                return View("File", model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File upload failed!!";
                return View("File", model);
            }
        }

        

        public ActionResult File()
        {
            ViewBag.Message = "Your File page.";
            ReviewModel model = new ReviewModel();

            return View(model);
        }

        public ActionResult Review()
        {
            ViewBag.Message = "Your Review page.";
            ReviewModel model = new ReviewModel();
            model.ImageURL = "";
            return View(model);
        }

        public ActionResult GetSentiment(string txtReviewCommnent)
        {
            ViewBag.Message = "Your Review page.";
            ReviewModel model = new ReviewModel();
            model.ImageURL = "";
            bool result = MakeRequest(new List<string> { txtReviewCommnent }).FirstOrDefault().Prediction.Value;
            if (result)
                model.ImageURL = "../Images/Positive.jpg";
            else
                model.ImageURL = "../Images/Negative.jpg";

            return View("Review", model);
        }

        public ActionResult GetSentimentForLocation(string SelectedLocation)
        {
            ViewBag.Message = "Your Review page.";
            ReviewModel model = new ReviewModel();
            switch (SelectedLocation)
            {
                case "1":
                    model.NegativeSentiments = "20%";
                    model.PositiveSentiments = "80%";
                    break;
                case "2":
                    model.NegativeSentiments = "15%";
                    model.PositiveSentiments = "85%";
                    break;
                case "3":
                    model.NegativeSentiments = "82%";
                    model.PositiveSentiments = "18%";
                    break;
                default:
                    break;
            }

            model.ShowSentiments = true;
            return View("Location", model);
        }

        public ActionResult Location()
        {
            ViewBag.Message = "Your Review page.";
            ReviewModel model = new ReviewModel();
            return View(model);
        }


        private List<SentimentPrediction> MakeRequest(List<string> text)
        {
            HttpClient client = new HttpClient();
            var objtimentAnalysisRequest = new SentimentAnalysisRequest();
            objtimentAnalysisRequest.Sentiments = new List<string>();
            objtimentAnalysisRequest.Sentiments.AddRange(text);

            var myContent = JsonConvert.SerializeObject(objtimentAnalysisRequest);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result =  client.PostAsync(@"http://localhost:5001/api/SentimentAnalysis", byteContent).Result;

            if (result.IsSuccessStatusCode)
            {
                var dataObjects = JsonConvert.DeserializeObject<SentimentAnalysisResponse>(result.Content.ReadAsStringAsync().Result);
                return dataObjects.SentimentPrediction;
            }
            else
                return null;
        }
    }

    public class SentimentAnalysisRequest
    {
        public List<string> Sentiments { get; set; }
    }

    public class SentimentAnalysisResponse
    {
        public List<SentimentPrediction> SentimentPrediction { get; set; }
    }


    
    public class SentimentPrediction
    {     
        public bool? Prediction { get; set; } 
               
        public float? Probability { get; set; }

        public float? Score { get; set; }
    }
}



  