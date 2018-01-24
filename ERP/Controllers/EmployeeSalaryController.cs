using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class EmployeeSalaryController : Controller
    {
        //
        // GET: /EmployeeSalary/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetList(int limit, int offset, string Name)
        {
            Business.Sys_EmployeeSalary Sys_EmployeeSalary = new Business.Sys_EmployeeSalary();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Name))
            {
                sb.Append(" and Name='" + Name + "'");
            }
            return Json(new { total = Sys_EmployeeSalary.GetUserAdminListCount(sb.ToString()), rows = Sys_EmployeeSalary.UserAdminList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Add() 
        {
            ViewData["SexListItems"] = GetSex();
            ViewData["GetMonthItems"] = GetMonthItems();
            return View();
        }

        [HttpPost]
        public ActionResult Add(Model.EmployeeSalary EmployeeSalary) 
        {
            ViewData["SexListItems"] = GetSex();
            ViewData["GetMonthItems"] = GetMonthItems();
            Business.Sys_EmployeeSalary Sys_EmployeeSalary = new Business.Sys_EmployeeSalary();
            Sys_EmployeeSalary.Add(EmployeeSalary);
            Response.Write("<script>parent.layer.closeAll();</script>");
            return View();
        }

        public ActionResult Edit() 
        {
            Business.Sys_EmployeeSalary Sys_EmployeeSalary = new Business.Sys_EmployeeSalary();
            Model.EmployeeSalary EmployeeSalary = Sys_EmployeeSalary.GetModel(Request["id"]);
            ViewData["SexListItems"] = GetSex(EmployeeSalary.Sex);
            ViewData["GetMonthItems"] = GetMonthItems(EmployeeSalary.Month.ToString());
            return View(EmployeeSalary);
        }

        [HttpPost]
        public ActionResult Edit(Model.EmployeeSalary EmployeeSalary)
        {
            Business.Sys_EmployeeSalary Sys_EmployeeSalary = new Business.Sys_EmployeeSalary();
            Sys_EmployeeSalary.Update(EmployeeSalary);
            ViewData["SexListItems"] = GetSex(EmployeeSalary.Sex);
            ViewData["GetMonthItems"] = GetMonthItems(EmployeeSalary.Month.ToString());
            Response.Write("<script>parent.layer.closeAll();</script>");
            return View();
        }

        public ActionResult Delete() 
        {
            Business.Sys_EmployeeSalary Sys_EmployeeSalary = new Business.Sys_EmployeeSalary();
            Sys_EmployeeSalary.Delete(Request["id"]);
            return Content("1");
        }

        public List<SelectListItem> GetMonthItems(string month="")
        {
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                SelectListItem items = new SelectListItem();
                items.Text = i.ToString();
                items.Value = i.ToString();
                if (!string.IsNullOrEmpty(month))
                {
                    if (i == Convert.ToInt32(month))
                    {
                        items.Selected = true;
                    }
                }
             
                deptSelectItems.Add(items);
            }
            return deptSelectItems;
        }

        public List<SelectListItem> GetSex(string Sex="")
        {
            
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();

            SelectListItem items = new SelectListItem();
            items.Text = "男";
            items.Value = "男";
            // items.Selected = true;
            deptSelectItems.Add(items);
            items = new SelectListItem();
            items.Text = "女";
            items.Value = "女";
           //items.Selected = true;
            deptSelectItems.Add(items);
            if (!string.IsNullOrEmpty(Sex))
            {
                if (Sex == "男")
                {
                    deptSelectItems[0].Selected = true;
                }
                else {
                    deptSelectItems[1].Selected = true;
                }
            }
            return deptSelectItems;
        }
	}
}