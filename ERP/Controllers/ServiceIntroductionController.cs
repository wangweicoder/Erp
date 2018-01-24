using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ERP.Controllers
{
    public class ServiceIntroductionController : Controller
    {
        //
        // GET: /ServiceIntroduction/
        public ActionResult Index()
        {
            Business.Sys_ServiceIntroduction Sys_ServiceIntroduction = new Business.Sys_ServiceIntroduction();
            return View(Sys_ServiceIntroduction.GetModel());
        }

        [ValidateInput(false)]
        public ActionResult UpdateInfo() 
        {
            string msg = Request["msg"];
            Business.Sys_ServiceIntroduction Sys_ServiceIntroduction = new Business.Sys_ServiceIntroduction();
            Sys_ServiceIntroduction.UpdateModel(msg);
            return Content("1");
        }


        public ActionResult ShowInfo()
        {
            Business.Sys_ServiceIntroduction Sys_ServiceIntroduction = new Business.Sys_ServiceIntroduction();
            ViewBag.info=Sys_ServiceIntroduction.GetModel().Msg;
            return View();
        }

        public ActionResult UploadFile() 
        {
            HttpPostedFileBase file = Request.Files["file"];
           
            ImgInfo ImgInfo = new ServiceIntroductionController.ImgInfo() 
            {
                src= Utility.ChangeText.SaveUploadPicture(file, "attach"),
            };
            ReturnUploadFileMsg ReturnUploadFileMsg = new ReturnUploadFileMsg() 
            {
               code=0,
               msg="上传成功",
               data = ImgInfo
            };
            string json = JsonConvert.SerializeObject(ReturnUploadFileMsg);
            return Content(json);
        }   

        public class ReturnUploadFileMsg 
        {
            public int code { set; get; }

            public string msg { set; get; }

            public ImgInfo data { set; get; }
        }

        public class ImgInfo 
        {
            public string src { set; get; }


            public string title { set; get; } 
        
        }
	}
}