using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class ChangingFlowerExpensesController : Controller
    {
        //
        // GET: /ChangingFlowerExpenses/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetList(int offset, string ReplacementUnit)
        {
            Business.Sys_ChangingFlowerExpenses Sys_ChangingFlowerExpenses = new Business.Sys_ChangingFlowerExpenses();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(ReplacementUnit))
            {
                sb.Append(" and ReplacementUnit='" + ReplacementUnit + "'");
            }
            return Json(new { total = Sys_ChangingFlowerExpenses.GetListCount(sb.ToString()), rows = Sys_ChangingFlowerExpenses.GetList(offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add() 
        {
            return View();
        }

        public ActionResult AddInfo(Model.ChangingFlowerExpenses ChangingFlowerExpenses)
        {
            Business.Sys_ChangingFlowerExpenses Sys_ChangingFlowerExpenses = new Business.Sys_ChangingFlowerExpenses();
            if (Sys_ChangingFlowerExpenses.InsertChangingFlowerExpenses(ChangingFlowerExpenses))
            {
                return Content("1");
            }
            return Content("0");
        }

        public ActionResult Edit() 
        {
            Business.Sys_ChangingFlowerExpenses Sys_ChangingFlowerExpenses = new Business.Sys_ChangingFlowerExpenses();
            return View(Sys_ChangingFlowerExpenses.GetModel(Request["ID"]));
        }

        public ActionResult EditInfo(Model.ChangingFlowerExpenses ChangingFlowerExpenses) 
        {
            Business.Sys_ChangingFlowerExpenses Sys_ChangingFlowerExpenses = new Business.Sys_ChangingFlowerExpenses();
            if (Sys_ChangingFlowerExpenses.UpdateChangingFlowerExpenses(ChangingFlowerExpenses))
            {
                return Content("1");
            }
            return Content("0");
        }

        public ActionResult DeleteInfo() 
        {
            Business.Sys_ChangingFlowerExpenses Sys_ChangingFlowerExpenses = new Business.Sys_ChangingFlowerExpenses();
            if (Sys_ChangingFlowerExpenses.DeleteInfo(Request["ID"]))
            {
                return Content("1");
            }
            return Content("0");
        }
	}
}