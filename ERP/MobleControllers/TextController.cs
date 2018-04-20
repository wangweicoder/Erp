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

        public ActionResult TextImg() 
        {
            string id = Request["id"];
            HttpPostedFileBase file = Request.Files["Filedata"];
            return Json(new { result = "OK", msg = "更换花卉成功" }, "text/html", JsonRequestBehavior.AllowGet);
        }
        public ActionResult yIndex()
        {
            return View();
        }
    }
}