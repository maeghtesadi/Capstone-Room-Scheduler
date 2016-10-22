using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapstoneRoomScheduler.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is Harambook";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Harambe's Home :'(";

            return View();
        }
    }
}