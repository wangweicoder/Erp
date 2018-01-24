using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class ContractCustomerController :LoginFilter
    {
        //
        // GET: /ContractCustomer/
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
        public JsonResult GetList(int offset, string RealName, string IsSign)
        {
            Business.Sys_ContractCustomer Sys_ContractCustomer = new Business.Sys_ContractCustomer();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(RealName))
            {
                sb.Append(" and RealName='" + RealName + "'");
            }
            if (!string.IsNullOrEmpty(IsSign))
            {
                sb.Append(" and IsSign='" + IsSign + "'");
            }
            List<Model.ContractCustomer> ContractCustomerList = Sys_ContractCustomer.GetList(offset, sb.ToString());
        
            return Json(new { total = Sys_ContractCustomer.GetListCount(sb.ToString()), rows = ContractCustomerList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            ViewData["GetSexList"] = GetSexList();
            ViewData["GetTypeList"] = GetTypeList();
            return View();
        }

        public ActionResult AddInfo(Model.ContractCustomer ContractCustomer)
        {
            Business.Sys_ContractCustomer Sys_ContractCustomer = new Business.Sys_ContractCustomer();
            ViewData["GetSexList"] = GetSexList();
            ViewData["GetTypeList"] = GetTypeList();
            if (Sys_ContractCustomer.AddInfo(ContractCustomer))
            {
                return Content("1");
            }
            return Content("0");
        }
        public ActionResult Edit()
        {
            Business.Sys_ContractCustomer Sys_ContractCustomer = new Business.Sys_ContractCustomer();
            Model.ContractCustomer ContractCustomer = Sys_ContractCustomer.GetInfo(Request["ID"]);
            ViewData["GetSexList"] = GetSexList(ContractCustomer.Sex);
            ViewData["GetTypeList"] = GetTypeList(ContractCustomer.IsSign);
            ViewData["ID"] = Request["ID"];
            return View(ContractCustomer);
        }
        public ActionResult EditInfo(Model.ContractCustomer ContractCustomer)
        {
            Business.Sys_ContractCustomer Sys_ContractCustomer = new Business.Sys_ContractCustomer();
            ViewData["GetSexList"] = GetSexList();
            ViewData["GetTypeList"] = GetTypeList();
            if (Sys_ContractCustomer.UpdateInfo(ContractCustomer))
            {
                return Content("1");
            }
            return Content("0");
        }
        public ActionResult DeleteInfo() 
        {
            Business.Sys_ContractCustomer Sys_ContractCustomer = new Business.Sys_ContractCustomer();
            if (Sys_ContractCustomer.DeleteInfo(Request["ID"]))
            {
                 return Content("1");
            }
            return Content("0");
        }
        public List<SelectListItem> GetTypeList(string type = "")
        {
            List<SelectListItem> hourList = new List<SelectListItem>();
            hourList.Add(new SelectListItem { Text = "未签约", Value = "未签约", Selected = true });
            hourList.Add(new SelectListItem { Text = "已签约", Value = "已签约" });
            hourList.Add(new SelectListItem { Text = "已锁定", Value = "已锁定" });
            if (string.IsNullOrEmpty(type))
            {
                if (type == "未签约")
                {
                    hourList[0].Selected = true;
                }
                if (type == "已签约")
                {
                    hourList[1].Selected = true;
                }
                if (type == "已锁定")
                {
                    hourList[2].Selected = true;
                }
            }
            return hourList;
        }

        public List<SelectListItem> GetSexList(string type = "")
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