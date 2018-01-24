using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class CustomRequirementsController : Controller
    {
        //
        // GET: /CustomRequirements/
        public ActionResult Index()
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            return View();
        }

        public JsonResult GetCustomRequirementsList(int offset, string State, string SelectItem)
        {
            Business.Sys_FlowerDemand Sys_FlowerDemand = new Business.Sys_FlowerDemand();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(State))
            {
                sb.Append(" and State='" + State + "'");
            }
            if (!string.IsNullOrEmpty(SelectItem))
            {
                sb.Append(" and FlowerCategoryType='" + SelectItem + "'");
            }
            return Json(new { total = Sys_FlowerDemand.GetFlowerDemandListCount(sb.ToString()), rows = Sys_FlowerDemand.GetList(offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }
        public List<SelectListItem> GetdeptSelectItems()
        {
            Business.Sys_FlowerCategory Sys_FlowerCategory = new Business.Sys_FlowerCategory();
            List<Model.FlowerCategory> FlowerCategoryList = Sys_FlowerCategory.GetListModel();
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            SelectListItem items = new SelectListItem();
            items.Text = "--请选择--";
            items.Value = "";
            items.Selected = true;
            deptSelectItems.Add(items);
            foreach (Model.FlowerCategory d in FlowerCategoryList)
            {
                SelectListItem item = new SelectListItem();
                item.Text = d.FlowerCategoryType;
                item.Value = d.id.ToString();
                item.Selected = false;
                deptSelectItems.Add(item);
            }
            return deptSelectItems;
        }

        public ActionResult Editstate() 
        {
            ViewData["id"] = Request["id"];
            return View();
        }
        
        public ActionResult Editstatemain() 
        {
            Business.Sys_FlowerDemand Sys_FlowerDemand = new Business.Sys_FlowerDemand();
            if (Sys_FlowerDemand.EditState(Request["state"],Request["id"]))
            {
                return Content("1");
            }
            return Content("2");
        }
	}
}