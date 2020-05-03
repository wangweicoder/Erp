using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.MobleControllers
{
    public class TextController : Controller
    {
        // GET: Text
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Img() 
        {
            ViewBag.url = Request["url"];
            return View();
        }
        public ActionResult yIndex()
        {
            return View();
        }
    }
}