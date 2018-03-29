using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class MenuController : LoginFilter
    {
        //
        [OutputCache(Duration=20,VaryByParam="none",Location=System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获得菜单集合
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="MenuCode"></param>
        /// <param name="MenuName"></param>
        /// <returns></returns>      
        public ActionResult GetMenuList(int limit, int offset, string MenuCode, string MenuName)
        {
            Business.Sys_Menu Sys_Menu = new Business.Sys_Menu();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(MenuCode))
            {
                sb.Append(" and MenuCode='" + MenuCode + "'");
            }
            if (!string.IsNullOrEmpty(MenuName))
            {
                sb.Append(" and MenuName='" + MenuName + "'");
            }
            return Json(new { total = Sys_Menu.GetMenuListCount(sb.ToString()), rows = Sys_Menu.MenuList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            return View();
        }
        [HttpPost]
        public ActionResult Add(Model.Menu Menu)
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            Business.Sys_Menu Sys_Menu = new Business.Sys_Menu();
            Menu.Terminal = Request["deptSelectItems"];
            if (Sys_Menu.InsertMenu(Menu))
            {
                Response.Write("<script>parent.layer.alert('添加成功!');</script>");
            }
            else
            {
                Response.Write("<script>parent.layer.alert('添加失败!');</script>");
            }
            return View();
        }

        public ActionResult Edit()
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            Business.Sys_Menu Sys_Menu = new Business.Sys_Menu();
            return View(Sys_Menu.GetMenuInfoById(Request["MenuId"]));
        }
        [HttpPost]
        public ActionResult Edit(Model.Menu Menu)
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            Business.Sys_Menu Sys_Menu = new Business.Sys_Menu();
            Menu.Terminal = Request["deptSelectItems"];
            if (Sys_Menu.EditMenu(Menu))
            {
                Response.Write("<script>parent.layer.alert('修改成功!');</script>");
            }
            else
            {
                Response.Write("<script>parent.layer.alert('修改失败!');</script>");
            }
            return View();
        }

        public ActionResult DeleteMenu() 
        {
            Business.Sys_Menu Sys_Menu = new Business.Sys_Menu();
            if (Sys_Menu.DeleteMenu(Request["MenuId"]))
            {
                return Content("True");
            }
            else
            {
                return Content("False");
            }
        }

        public List<SelectListItem> GetdeptSelectItems()
        {
            Business.Sys_Role Sys_Role = new Business.Sys_Role();
            List<Model.RoleInfo> RoleInfoList = Sys_Role.UserRoleList();
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Text = "PC";
            item.Value = "PC";
            deptSelectItems.Add(item);
            item = new SelectListItem();
            item.Text = "手机";
            item.Value = "手机";
            deptSelectItems.Add(item);
            return deptSelectItems;
        }
	}
}