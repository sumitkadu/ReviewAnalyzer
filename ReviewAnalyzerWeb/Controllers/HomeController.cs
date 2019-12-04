using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Review_Analyzer.Models;

namespace Review_Analyzer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
            if (txtReviewCommnent.Length % 2 == 0)
                model.ImageURL = "../Images/Positive.jpg";
            else
                model.ImageURL = "../Images/Negative.jpg";
            return View("Review",model);
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
    }
}