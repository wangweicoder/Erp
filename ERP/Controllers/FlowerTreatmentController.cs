using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class FlowerTreatmentController : LoginFilter
    {
        //
        // GET: /FlowerTreatment/
        public ActionResult Index()
        {
            ViewData["SelectItem"] = GetFlowerTreatmentName();
            return View();
        }
        public List<SelectListItem> GetFlowerTreatmentName()
        {
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();

            List<Model.FlowerTreatment> FlowerTreatment = Sys_FlowerTreatment.GetFlowerTreatmentName();
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Text = "-请选择-";
            item.Value = "";
            item.Selected = true;
            deptSelectItems.Add(item);
            foreach (Model.FlowerTreatment d in FlowerTreatment)
            {
                item = new SelectListItem();
                item.Text = d.OwnedUsersRealName;
                item.Value = d.OwnedUsersId.ToString();
                item.Selected = false;
                deptSelectItems.Add(item);
            }
            return deptSelectItems;
        }
        public ActionResult GetList(int limit, int offset, string FlowerNumber, string deptSelectItems)
        {
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            StringBuilder sb = new StringBuilder();
            //sb.Append(" and OwnedUsersId='"+Utility.ChangeText.GetUsersId()+"' ");
            if (!string.IsNullOrEmpty(FlowerNumber))
            {
                sb.Append(" and FlowerNumber='" + FlowerNumber + "'");
            }
            if (!string.IsNullOrEmpty(deptSelectItems))
            {
                sb.Append(" and OwnedUsersId='" + deptSelectItems + "'");
            }
            return Json(new { total = Sys_FlowerTreatment.GetFlowerTreatmentListCount(sb.ToString()), rows = Sys_FlowerTreatment.FlowerTreatmentList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit() 
        {
            string id = Request["id"];
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            ViewData["GetUserInfoSelectItems"] = GetUserInfoSelectItems();
            return View(Sys_FlowerTreatment.GetModel(id));
        }

        public ActionResult MIndex()
        {
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            StringBuilder sb = new StringBuilder();
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByUserId(Utility.ChangeText.GetUsersId());
            if (UserAdmin.RoleCode != "Customer")
            {
                sb.Append(" and UsersId='" + Utility.ChangeText.GetUsersId() + "'");
            }
            else if (Utility.ChangeText.GetUserName() != "admin")
            {
                sb.Append(" and OwnedUsersId='" + Utility.ChangeText.GetUsersId() + "'");
            }
            return View(Sys_FlowerTreatment.FlowerTreatmentList(0, 1, sb.ToString()));
        }

        public ActionResult GetMobleListMore()
        {
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            StringBuilder sb = new StringBuilder();
            Business.Sys_UserAdmin Sys_UserAdmin=new Business.Sys_UserAdmin ();
            Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByUserId(Utility.ChangeText.GetUsersId());
            if (UserAdmin.RoleCode != "Customer")
            {
                sb.Append(" and UsersId='" + Utility.ChangeText.GetUsersId() + "'");
            }
            else if (Utility.ChangeText.GetUserName() != "admin")
            {
                sb.Append(" and OwnedUsersId='" + Utility.ChangeText.GetUsersId() + "'");  
            }
            Utility.Log.WriteTextLog("testsql", "", "", Request["page"], sb.ToString());
            int page = int.Parse(Request["page"]);
            if (page > 1)
            {
                page = (page - 1) * 10 + 1;
            }
            List<Model.FlowerTreatment> List = Sys_FlowerTreatment.FlowerTreatmentList(0, Convert.ToInt32(page), sb.ToString());
            return Content(JsonConvert.SerializeObject(List));
        }


        public ActionResult EditInfo(Model.FlowerTreatment FlowerTreatment) 
        {
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByUserId(Convert.ToInt32(FlowerTreatment.OwnedUsersId));
            Model.UserAdmin UserAdmin1 = Sys_UserAdmin.GetUserAdminByUserId(FlowerTreatment.UsersId);
            FlowerTreatment.UserRealName = UserAdmin1.RealName;
            FlowerTreatment.OwnedUsersRealName = UserAdmin.RealName;
            if (Sys_FlowerTreatment.Update(FlowerTreatment))
            {
                return Content("1");
            }
            return Content("0");
        }

        public ActionResult DeleteInfo() 
        {
            string id = Request["id"];
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            if (Sys_FlowerTreatment.DeleteInfo(id))
            {
                return Content("1");
            }
            return Content("0");
        }

        public List<SelectListItem> GetUserInfoSelectItems()
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            List<Model.UserAdmin> list = Sys_UserAdmin.GetUserAdminListByRoleCodeNo("Customer");
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            foreach (Model.UserAdmin d in list)
            {
                item = new SelectListItem();
                item.Text = d.RealName;
                item.Value = d.ID.ToString();
                item.Selected = true;
                deptSelectItems.Add(item);
            }
            return deptSelectItems;
        }
    }
}