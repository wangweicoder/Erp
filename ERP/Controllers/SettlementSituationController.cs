using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class SettlementSituationController : LoginFilter
    {
        //
        // GET: /SettlementSituation/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">从第几条数据开始</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public JsonResult GetList(int offset, string CompanyName, string DutyParagraph, string WhatMonth, string Isrenew)
        {
            Business.Sys_SettlementSituation Sys_SettlementSituation = new Business.Sys_SettlementSituation();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(CompanyName))
            {
                sb.Append(" and CompanyName='" + CompanyName + "'");
            }
            if (!string.IsNullOrEmpty(DutyParagraph))
            {
                sb.Append(" and DutyParagraph='" + DutyParagraph + "'");
            }
            if (!string.IsNullOrEmpty(WhatMonth))
            {
                sb.Append(" and WhatMonth='" + WhatMonth + "'");
            }
            if (!string.IsNullOrEmpty(Isrenew))
            {
                sb.Append(" and Isrenew='" + Isrenew + "'");
            }
            return Json(new { total = Sys_SettlementSituation.GetListCount(sb.ToString()), rows = Sys_SettlementSituation.GetList(offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add() 
        {
            ViewData["GetPayTypeList"] = GetPayTypeList();
            ViewData["GetBillTypeTypeList"] = GetBillTypeTypeList();
            ViewData["GetWhatMonthTypeList"] = GetWhatMonthTypeList();
            
            ViewData["GetIsrenewTypeList"] = GetIsrenewTypeList();
            return View();
        }

        public ActionResult AddInfo(Model.SettlementSituation SettlementSituation)
        {
            Business.Sys_SettlementSituation Sys_SettlementSituation = new Business.Sys_SettlementSituation();
            if (Sys_SettlementSituation.AddInfo(SettlementSituation))
            {
                return Content("1");
            }
            return Content("0");
        }


        public ActionResult Edit() 
        {
            Business.Sys_SettlementSituation Sys_SettlementSituation = new Business.Sys_SettlementSituation();
            Model.SettlementSituation SettlementSituation = Sys_SettlementSituation.GetInfo(Request["ID"]);
            ViewData["GetPayTypeList"] = GetPayTypeList(SettlementSituation.PayType);
            ViewData["GetBillTypeTypeList"] = GetBillTypeTypeList(SettlementSituation.BillType);
            ViewData["GetIsrenewTypeList"] = GetIsrenewTypeList(SettlementSituation.Isrenew);
            ViewData["GetWhatMonthTypeList"] = GetWhatMonthTypeList();
            return View(SettlementSituation);
        }

        public ActionResult EditInfo(Model.SettlementSituation SettlementSituation)
        {
            Business.Sys_SettlementSituation Sys_SettlementSituation = new Business.Sys_SettlementSituation();
            if (Sys_SettlementSituation.UpdateInfo(SettlementSituation))
            {
                return Content("1");
            }
            return Content("0");
        }

        public ActionResult DeleteInfo() 
        {
            Business.Sys_SettlementSituation Sys_SettlementSituation = new Business.Sys_SettlementSituation();
            if (Sys_SettlementSituation.DeleteInfo(Request["ID"]))
            {
                return Content("1");
            }
            return Content("0");
        }
        public List<SelectListItem> GetPayTypeList(string type = "")
        {
            List<SelectListItem> hourList = new List<SelectListItem>();
            hourList.Add(new SelectListItem { Text = "季预付", Value = "季预付", Selected = true });
            hourList.Add(new SelectListItem { Text = "季后付", Value = "季后付" });
            if (string.IsNullOrEmpty(type))
            {
                if (type == "季预付")
                {
                    hourList[0].Selected = true;
                }
                if (type == "季后付")
                {
                    hourList[1].Selected = true;
                }
            }
            return hourList;
        }

        public List<SelectListItem> GetBillTypeTypeList(string type = "")
        {
            List<SelectListItem> hourList = new List<SelectListItem>();
            hourList.Add(new SelectListItem { Text = "普票", Value = "普票", Selected = true });
            hourList.Add(new SelectListItem { Text = "专票", Value = "专票" });
            if (string.IsNullOrEmpty(type))
            {
                if (type == "普票")
                {
                    hourList[0].Selected = true;
                }
                if (type == "专票")
                {
                    hourList[1].Selected = true;
                }
            }
            return hourList;
        }


        public List<SelectListItem> GetIsrenewTypeList(string type = "")
        {
            List<SelectListItem> hourList = new List<SelectListItem>();
            hourList.Add(new SelectListItem { Text = "是", Value = "是", Selected = true });
            hourList.Add(new SelectListItem { Text = "否", Value = "否" });
            if (string.IsNullOrEmpty(type))
            {
                if (type == "是")
                {
                    hourList[0].Selected = true;
                }
                if (type == "否")
                {
                    hourList[1].Selected = true;
                }
            }
            return hourList;
        }
        public List<SelectListItem> GetWhatMonthTypeList(string type = "")
        {
            List<SelectListItem> hourList = new List<SelectListItem>();
            hourList.Add(new SelectListItem { Text = "1", Value = "1", Selected = true });
            hourList.Add(new SelectListItem { Text = "2", Value = "2" });
            hourList.Add(new SelectListItem { Text = "3", Value = "3" });
            hourList.Add(new SelectListItem { Text = "4", Value = "4" });
            hourList.Add(new SelectListItem { Text = "5", Value = "5" });
            hourList.Add(new SelectListItem { Text = "6", Value = "6" });
            hourList.Add(new SelectListItem { Text = "7", Value = "7" });
            hourList.Add(new SelectListItem { Text = "8", Value = "8" });
            hourList.Add(new SelectListItem { Text = "9", Value = "9" });
            hourList.Add(new SelectListItem { Text = "10", Value = "10" });
            hourList.Add(new SelectListItem { Text = "11", Value = "11" });
            hourList.Add(new SelectListItem { Text = "12", Value = "12" });
         
            if (string.IsNullOrEmpty(type))
            {
                if (type == "1")
                {
                    hourList[0].Selected = true;
                }
                if (type == "2")
                {
                    hourList[1].Selected = true;
                }
                if (type == "3")
                {
                    hourList[2].Selected = true;
                }
                if (type == "4")
                {
                    hourList[3].Selected = true;
                }
                if (type == "5")
                {
                    hourList[4].Selected = true;
                }
                if (type == "6")
                {
                    hourList[5].Selected = true;
                }
                if (type == "7")
                {
                    hourList[6].Selected = true;
                }
                if (type == "8")
                {
                    hourList[7].Selected = true;
                }
                if (type == "9")
                {
                    hourList[8].Selected = true;
                }
                if (type == "10")
                {
                    hourList[9].Selected = true;
                }
                if (type == "11")
                {
                    hourList[10].Selected = true;
                }
                if (type == "12")
                {
                    hourList[11].Selected = true;
                }
            }
            return hourList;
        }
        
	}
}