using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class ClockAttendanceController : Controller
    {
        //
        // GET: /ClockAttendance/
        public ActionResult Index()
        {
            ViewData["UserListInfo"] = GetUserInfoSelectItems();
            return View();
        }

        public ActionResult GetList()
        {
            int offset = Convert.ToInt32(Request["offset"]);
            string UserName = Request["UserName"];
            Business.Sys_ClockAttendance Sys_ClockAttendance = new Business.Sys_ClockAttendance();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(UserName))
            {
                sb.Append(" and UserName='" + UserName + "'");
            }
            return Json(new { total = Sys_ClockAttendance.GetClockAttendanceListCount(sb.ToString()), rows = Sys_ClockAttendance.UserClockAttendanceList( offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetUserInfoSelectItems()
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            List<Model.UserAdmin> list = Sys_UserAdmin.GetUserAdminListByRoleCodeNo("Customer");
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Text = "-请选择-";
            item.Value = "";
            item.Selected = true;
            deptSelectItems.Add(item);
            foreach (Model.UserAdmin d in list)
            {
                item = new SelectListItem();
                item.Text = d.RealName;
                item.Value = d.ID.ToString();
                item.Selected = false;
                deptSelectItems.Add(item);
            }
            return deptSelectItems;
        }

	}
}