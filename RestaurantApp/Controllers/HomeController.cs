using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using DevExpressProjectTemplate.Web;

namespace RestaurantApp.Controllers
{
    public class HomeController : Controller

    {
        public static string GlobalColorScheme { get; set; }
        [AllowAnonymous]
        public ActionResult Index()
        {


            //ASPxWebClientUIControl.GlobalColorScheme = "carmine";
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
    }
}