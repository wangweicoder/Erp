﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class AdviertisementController : Controller
    {
        // GET: Adviertisement
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">从第几条数据开始</param>
        /// <param name="AdTitle">标题</param>        
        /// <returns></returns>
        public JsonResult GetAdviertisementList(int limit, int offset, string Title)
        {
            Business.Sys_Adviertisement Sys_UserLog = new Business.Sys_Adviertisement();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Title))
            {
                sb.Append(" and Title='" + Title + "'");
            }           

            return Json(new { total = Sys_UserLog.GetAdviertisementListCount(sb.ToString()), rows = Sys_UserLog.GetAdviertisementList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add()
        {           
            return View();
        }
        [HttpPost]
        public ActionResult Add(Model.Adviertisement adv)
        {
            try
            {
                Business.Sys_Adviertisement Sys_Adviertisement = new Business.Sys_Adviertisement();               
                HttpPostedFileBase file = Request.Files["attach_path"];               
                adv.Picture = Utility.ChangeText.SaveUploadPicture(file, "AdvPic");
                adv.CreateTime = DateTime.Now;
                adv.UpdateTime = DateTime.Now;
                Sys_Adviertisement.InsertAdviertisement(adv);
                Response.Write("<script>parent.layer.closeAll();</script>");
                return View();               
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog("报错", "Adviertisement", "Add", "后台提交广告", "");
                return null;
            }
        }
        /// <summary>
        /// 删除一个广告信息
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteAdvInfo()
        {
            int ID = int.Parse(Request["AdvId"]);
            Business.Sys_Adviertisement Sys_Adviertisement = new Business.Sys_Adviertisement();
            if (Sys_Adviertisement.DeleteAdviertisement(ID))
            {
                return Content("True");
            }
            return Content("False");
        }
        public ActionResult testSlide()
        {
            return View();
        }
        /// <summary>
        /// 三个广告
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAdvList()
        {
            Business.Sys_Adviertisement Sys_Adviertisement = new Business.Sys_Adviertisement();
            List<Model.Adviertisement> FlowerList = Sys_Adviertisement.GetAdviertisementList(3,0,"");
            return Json(FlowerList, JsonRequestBehavior.AllowGet);
        }
    }
}