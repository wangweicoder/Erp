using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class SalesmanController : LoginFilter
    {
        //
        // GET: /Salesman/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(int offset, string RealName, string Phone)
        {
            Business.Sys_Salesman Sys_Salesman = new Business.Sys_Salesman();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(RealName))
            {
                sb.Append(" and RealName='" + RealName + "'");
            }
            if (!string.IsNullOrEmpty(Phone))
            {
                sb.Append(" and Phone='" + Phone + "'");
            }
           List<Model.Salesman> SalesmanList= Sys_Salesman.SalesmanList(offset, sb.ToString());
       
        
           return Json(new { total = Sys_Salesman.GetSalesmanListCount(sb.ToString()), rows = SalesmanList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult add()
        {
            ViewData["GetSexList"] = GetSexList();
            return View();
        }
        public ActionResult Edit()
        {
            Business.Sys_Salesman Sys_Salesman = new Business.Sys_Salesman();
            Model.Salesman Salesman = Sys_Salesman.GetInfo(Request["ID"]);
            ViewData["GetSexList"] = GetSexList(Salesman.Sex);
            return View(Salesman);
        }
        public ActionResult AddInfo(Model.Salesman Saleman)
        {
            ViewData["GetSexList"] = GetSexList();
            Business.Sys_Salesman Sys_Salesman = new Business.Sys_Salesman();
            if (Sys_Salesman.AddSalesman(Saleman))
            {
                return Content("1");
            }
            return Content("0");
        }

        public ActionResult EditInfo(Model.Salesman Saleman)
        {
            ViewData["GetSexList"] = GetSexList();
            Business.Sys_Salesman Sys_Salesman = new Business.Sys_Salesman();
            if (Sys_Salesman.EditSalesman(Saleman))
            {
                return Content("1");
            }
            return Content("0");
        }

        public ActionResult DeleteInfo(string id)
        {
            Business.Sys_Salesman Sys_Salesman = new Business.Sys_Salesman();
            if (Sys_Salesman.DeleteSalesman(id))
            {
                return Content("1");
            }
            return Content("0");
        }



        public List<SelectListItem> GetSexList(string type="")
        {
            List<SelectListItem> hourList = new List<SelectListItem>();
            hourList.Add(new SelectListItem { Text = "男", Value = "男", Selected = true });
            hourList.Add(new SelectListItem { Text = "女", Value = "女" });
            if (string.IsNullOrEmpty(type))
            {
                if (type == "男")
                {
                    hourList[0].Selected = true;
                }
                if (type == "女")
                {
                    hourList[1].Selected = true;
                }
            }
            return hourList;
        }
    }
}