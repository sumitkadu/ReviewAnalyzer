using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult UploadFile(HttpPostedFileBase uploadFile)
        {
            ViewBag.Message = "Your application description page.";
            ReviewModel model = new ReviewModel();
            try
            {
                if (uploadFile.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(uploadFile.FileName);
                    // string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    // uploadFile.SaveAs(_path);
                    //TODO: Send file to get review
                    model.NegativeSentiments = "20%";
                    model.PositiveSentiments = "80%";
                    model.ShowSentiments = true;
                }

                return View("File", model);
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View("Location", model);
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