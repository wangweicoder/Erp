using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ERP.Controllers
{
    public class FlowerChangeController : LoginFilter
    {
        //
        // GET: /FlowerChange/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(int limit, int offset, string FlowerNumber, string ChangeNumer, string State)
        {
            Business.Sys_FlowerChange Sys_FlowerChange = new Business.Sys_FlowerChange();
            StringBuilder sb = new StringBuilder();
            //sb.Append(" and OwnedUsersId='" + Utility.ChangeText.GetUsersId() + "' ");
            if (!string.IsNullOrEmpty(FlowerNumber))
            {
                sb.Append(" and FlowerNumber='" + FlowerNumber + "'");
            }
            if (!string.IsNullOrEmpty(ChangeNumer))
            {
                sb.Append(" and Number='" + ChangeNumer + "'");
            }
            if (!string.IsNullOrEmpty(State))
            {
                  sb.Append(" and State='" + State + "'");
            }
            return Json(new { total = Sys_FlowerChange.GetFlowerChangeListCount(sb.ToString()), rows = Sys_FlowerChange.GetList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MIndex()
        {
            Business.Sys_FlowerChange Sys_FlowerChange = new Business.Sys_FlowerChange();
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
        
            return View(Sys_FlowerChange.GetList(0,1, sb.ToString()));
        }


        public ActionResult GetMobleListMore()
        {
            Business.Sys_FlowerChange Sys_FlowerChange = new Business.Sys_FlowerChange();
            StringBuilder sb = new StringBuilder();
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByUserId(Utility.ChangeText.GetUsersId());
            if (UserAdmin.RoleCode != "Customer")
            {
                sb.Append(" and UsersId='" + Utility.ChangeText.GetUsersId() + "'");
            }
            else  if (Utility.ChangeText.GetUserName() != "admin")
            {
                sb.Append(" and OwnedUsersId='" + Utility.ChangeText.GetUsersId() + "'");
            }
         
            int page =int.Parse(Request["page"]);
            if (page>1)
            {
                page = (page - 1) * 10 + 1;
            }
            List<Model.FlowerChange> List=  Sys_FlowerChange.GetList(0, Convert.ToInt32(page), sb.ToString()); 
            return Content(JsonConvert.SerializeObject(List));
        }
	}
}