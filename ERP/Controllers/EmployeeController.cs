using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class EmployeeController : LoginFilter
    {
        //
        // GET: /Employee/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 分页获得集合数据
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="Number"></param>
        /// <param name="Name"></param>
        /// <param name="DataType"></param>
        /// <returns></returns>
        public JsonResult GetList(int limit, int offset, string Number, string Name, string DataType)
        {
            Business.Sys_Employee Sys_Employee = new Business.Sys_Employee();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Number))
            {
                sb.Append(" and Number='" + Number + "'");
            }
            if (!string.IsNullOrEmpty(Name))
            {
                sb.Append(" and Name='" + Name + "'");
            }
            if (!string.IsNullOrEmpty(DataType))
            {
                sb.Append(" and DataType='" + DataType + "'");
            }
            return Json(new { total = Sys_Employee.GetCount(sb.ToString()), rows = Sys_Employee.GetList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 员工入职页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployeeEntry()
        {
            if (Request["Number"] != null)//修改
            {
                if (Request["IsShowInfo"] != null)//查看详情
                {
                    ViewData["IsShowInfo"] = "True";
                }
                Business.Sys_EmployeeEntry Sys_EmployeeEntry = new Business.Sys_EmployeeEntry();
                return View(Sys_EmployeeEntry.GetInfoByNumber(Request["Number"]));
            }
            return View();
        }

        /// <summary>
        /// 写入员工入职表信息
        /// </summary>
        /// <param name="EmployeeEntry"></param>
        /// <returns></returns>                                                                                       
        [HttpPost]
        public ActionResult EmployeeEntry(Model.EmployeeEntry EmployeeEntry)
        {
            Business.Sys_EmployeeEntry Sys_EmployeeEntry = new Business.Sys_EmployeeEntry();
            if (!string.IsNullOrEmpty(EmployeeEntry.Number))
            {
                Sys_EmployeeEntry.UpdateEmployeeEntry(EmployeeEntry);
                Response.Write("<script>parent.layer.alert('修改成功!');</script>");
            }
            else {
                EmployeeEntry.Number = Utility.ChangeText.OrderIdCreate();
                if (Sys_EmployeeEntry.InsertEmployeeEntry(EmployeeEntry))
                {
                    Response.Write("<script>parent.layer.alert('添加成功!');</script>");
                }
            }
            //页面待定 调往订单详情页面
            return View();
        }

        /// <summary>
        /// 员工转正
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployeeTurnover()
        {
            ViewData["add"] = "true";
            if (Request["Number"] != null)
            {
                ViewData["add"] = "false";
                if (Request["IsShowInfo"] != null)//查看详情
                {
                    ViewData["IsShowInfo"] = "True";
                }
                Business.Sys_EmployeeTurnover Sys_EmployeeTurnover = new Business.Sys_EmployeeTurnover();
                Model.EmployeeTurnover EmployeeTurnover = Sys_EmployeeTurnover.GetInfoByNumber(Request["Number"]);
                return View(EmployeeTurnover);
            }
            return View();
        }

        /// <summary>
        /// 写入员工转正信息
        /// </summary>
        /// <param name="EmployeeTurnover"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EmployeeTurnover(Model.EmployeeTurnover EmployeeTurnover)
        {
            Business.Sys_EmployeeTurnover Sys_EmployeeTurnover = new Business.Sys_EmployeeTurnover();
            if (!string.IsNullOrEmpty(EmployeeTurnover.Number))
            {
                Sys_EmployeeTurnover.UpdateInfo(EmployeeTurnover);
                Response.Write("<script>parent.layer.alert('修改成功');</script>");
            }
            else
            {
                EmployeeTurnover.CreateTime = DateTime.Now;
                EmployeeTurnover.Number = Utility.ChangeText.OrderIdCreate();
                if (Sys_EmployeeTurnover.InsertEmployeeTurnover(EmployeeTurnover))
                {
                    Response.Write("<script>parent.layer.alert('添加成功!');</script>");
                }
            }
            //页面待定 调往订单详情页面
            return View();
        }


        public ActionResult DeleteInfo() 
        {
            string id = Request["id"];
            Business.Sys_Employee Sys_Employee = new Business.Sys_Employee();

            Sys_Employee.DeleteInfo(id);
            return Content("1");
        }
    }
}