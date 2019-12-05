using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Review_Analyzer.Models
{
    public class ReviewModel
    {
        public string ImageURL { get; set; }
        public List<SelectListItem> Locations
        {
            get
            {
                return new List<SelectListItem> { new SelectListItem { Text = "Location1", Value = "1" },
                    new SelectListItem { Text = "Location2", Value = "2" },
                    new SelectListItem { Text = "Location3", Value = "3" } };
            }
        }

        public string SelectedLocation { get; set; }

        public string PositiveSentiments { get; set; }
        public string NegativeSentiments { get; set; }

        public bool ShowSentiments { get; set; }

    }
}