using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class UsersLoginLogController : LoginFilter
    {
        //
        // GET: /UserInfo1/
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
        public JsonResult GetUserList(int limit, int offset, string UserLoginMonth, string SelectItem, string UsersSelectItems)
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(UserLoginMonth))
            {
                sb.Append(" and UserName='" + UserLoginMonth + "'");
            }
            if (!string.IsNullOrEmpty(SelectItem))
            {
                 sb.Append(" and RoleCode='" + SelectItem + "'");
            }
            if (!string.IsNullOrEmpty(UsersSelectItems))
            {
                sb.Append(" and  charindex(',"+UsersSelectItems+",',','+cast(WorkUsersId as varchar)+','  )>0 ");
            }
        
            return Json(new { total = Sys_UserAdmin.GetUserAdminListCount(sb.ToString()), rows = Sys_UserAdmin.UserAdminList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }   
        
        /// <summary>
        /// 删除一个会员
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteUserAdminInfo()
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            int ID = int.Parse(Request["UsersId"]);
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            if (Sys_UserAdmin.DeleteUserAdminInfo(ID))
            {
                return Content("True");
            }
            return Content("False");
        }

        public ActionResult Edit()
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            int UserAdminId = int.Parse(Request["UserAdminId"]);
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            return View(Sys_UserAdmin.GetUserAdminByUserId(UserAdminId));
        }

        public ActionResult EditInfo(Model.UserAdmin UserAdmin)
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin(); 
            Business.Sys_Role Sys_Role = new Business.Sys_Role();
            UserAdmin.PassWord = Utility.ChangeText.md5(UserAdmin.PassWord);
            UserAdmin.RoleName = Sys_Role.GetRoleInfoByRoleCode(UserAdmin.RoleCode).RoleName;
            if (Sys_UserAdmin.UpdateUserAdmin(UserAdmin))
            {
                return Content("1");
            }
            return Content("0");
        }

        public ActionResult Add()
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            return View();
        }


        public ActionResult AddInfo(Model.UserAdmin UserAdmin)
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            Business.Sys_Role Sys_Role = new Business.Sys_Role();           
            UserAdmin.RoleName=Sys_Role.GetRoleInfoByRoleCode(UserAdmin.RoleCode).RoleName;
            UserAdmin.PassWord = Utility.ChangeText.md5(UserAdmin.PassWord);
            if (Sys_UserAdmin.InsertUserAdmin(UserAdmin))
            {
                return Content("1");
            }
            return Content("0");
        }


        /// <summary>
        /// 验证管理员账号是否存在
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckUserAdminInfo()
        {
            string UserName = Request["UserName"];
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            if (Sys_UserAdmin.CheckUserAdminInfo(UserName))
            {
                return Content("True");
            }
            return Content("False");
        }

        public ActionResult SetRoleInfo() 
        {
            ViewData["SetRoleInfoId"] = Request["SetRoleInfoId"];
            ViewData["deptSelectItems"] = GetdeptSelectItems();
        

            return View();
        }


        [HttpPost]
        public ActionResult SetRoleInfo(string deptSelectItems)
        {
            ViewData["SetRoleInfoId"] = Request["SetRoleInfoId"];
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            string RoleCode = Request["deptSelectItems"];
            Business.Sys_Role Sys_Role = new Business.Sys_Role();
            Model.RoleInfo RoleInfo = Sys_Role.GetRoleInfoByRoleCode(RoleCode);
            string SetRoleInfoId = Request["SetRoleInfoId"];
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();        
            if (Sys_UserAdmin.SetUserAdminRole(SetRoleInfoId, RoleInfo.RoleCode, RoleInfo.RoleName))
            {
               
            }
            Response.Write("<script>parent.layer.closeAll();</script>");
            return View();
        }

        public List<SelectListItem> GetUserInfoSelectItems() 
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            List<Model.UserAdmin>  list= Sys_UserAdmin.GetUserAdminListByRoleCodeNo("Customer");
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
        public List<SelectListItem> GetdeptSelectItems()
        {
            Business.Sys_Role Sys_Role = new Business.Sys_Role();
            List<Model.RoleInfo> RoleInfoList = Sys_Role.UserRoleList();
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Text = "-请选择-";
            item.Value = "";
            item.Selected = true;
            deptSelectItems.Add(item);
            foreach (Model.RoleInfo d in RoleInfoList)
            {
                item = new SelectListItem();
                item.Text = d.RoleName;
                item.Value = d.RoleCode.ToString();
                item.Selected = false;
                deptSelectItems.Add(item);
            }
            return deptSelectItems;
        }   
       
	}
}