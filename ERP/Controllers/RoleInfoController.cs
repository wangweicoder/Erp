using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class RoleInfoController : LoginFilter
    {
        //
        // GET: /RoleInfo/
        public ActionResult Index()
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            return View();
        }
        public ActionResult GetRoleList(int limit, int offset, string SelectItem)
        {
            Business.Sys_Role Sys_Role = new Business.Sys_Role();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SelectItem))
            {
                sb.Append(" and RoleCode='" + SelectItem + "'");
            }
            return Json(new { total = Sys_Role.GetRoleLisCount(sb.ToString()), rows = Sys_Role.UserRoleList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckRoleInfo()
        {
            Business.Sys_Role Sys_Role = new Business.Sys_Role();
            if (Sys_Role.CheckRoleInfo(Request["RoleCode"], Request["RoleName"]))
            {
                return Content("True");
            } return Content("False");
        }


        public ActionResult Add()
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            return View();
        }

        [HttpPost]
        public ActionResult Add(Model.RoleInfo RoleInfo)
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            Business.Sys_Role Sys_Role = new Business.Sys_Role();
            if (Sys_Role.InsertRole(RoleInfo))
            {
                Response.Write("<script>parent.layer.closeAll();</script>");
            }
            else
            {
                Response.Write("<script>parent.layer.closeAll();</script>");
            }
            return View();
        }

        public ActionResult SetMenuInfo()
        {
            Business.Sys_Role Sys_Role = new Business.Sys_Role();
            Model.RoleInfo RoleInfo = Sys_Role.GetRoleInfoByRoleID(Request["RoleId"]);
            string UsersId = Request["UsersId"];
            string[] MenuList = new string[1000000];
            if (!string.IsNullOrEmpty(RoleInfo.MenuIdList))
            {
                MenuList = RoleInfo.MenuIdList.Split(',');
            }
            Business.Sys_Menu Sys_Menu = new Business.Sys_Menu();
            List<Model.Menu> MenuParentList = Sys_Menu.GetMenuListByHierarchy("1");
            List<Model.Menu> MenuChildrenList = Sys_Menu.GetMenuListByHierarchy("2");
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"panel-body\" style=\"padding-bottom:0px;\">");
            foreach (var item in MenuParentList)
            {
                bool CheckParent = false;
                sb.Append("<div class=\"panel panel-default\">");
                for (int i = 0; i < MenuList.Length; i++)
                {
                    if (int.Parse(MenuList[i]) == item.id)
                    {
                        CheckParent = true;
                        sb.Append("<div class=\"panel-heading\"><input type=\"checkbox\" checked=\"checked\" class=\"" + item.MenuCode + "\" id=\"" + item.MenuCode + "\" name=\"MenuListInfo\" onclick=\"FillCheckBox(this)\"  value=\"" + item.id + "\"/>" + item.MenuName + "(" + item.Terminal + ")");
                        break;
                    }
                }
                if (!CheckParent)
                {
                    sb.Append("<div class=\"panel-heading\"><input type=\"checkbox\" class=\"" + item.MenuCode + "\" id=\"" + item.MenuCode + "\" name=\"MenuListInfo\" onclick=\"FillCheckBox(this)\"  value=\"" + item.id + "\"/>" + item.MenuName + "(" + item.Terminal + ")");
                }
                sb.Append("<div class=\"panel-body\"><div id=\"formSearch\" div=\"form-horizontal\">");
                List<Model.Menu> MenuChildrenListByItem = MenuChildrenList.Where(x => x.SuperiorMenuID == item.id).ToList();

                foreach (var items in MenuChildrenListByItem)
                {
                    bool CheckChildren = false;
                    sb.Append("<label class=\"control-label col-sm-2\" for=\"txt_search_departmentname\">");
                    for (int i = 0; i < MenuList.Length; i++)
                    {
                        if (int.Parse(MenuList[i]) == items.id)
                        {
                            CheckChildren = true;
                            sb.Append("<input name=\"MenuListInfo\" type=\"checkbox\"  checked=\"checked\" class=\"" + item.MenuCode + "\" onclick=\"CheckBoxChilrden(this)\" value=\"" + items.id + "\" />" + items.MenuName + "");
                            break;
                        }
                    }
                    if (!CheckChildren)
                    {
                        sb.Append("<input name=\"MenuListInfo\" type=\"checkbox\"  class=\"" + item.MenuCode + "\" onclick=\"CheckBoxChilrden(this)\" value=\"" + items.id + "\" />" + items.MenuName + "");
                    }
                    sb.Append("</label>");
                }
                sb.Append("</div></div></div></div>");
            }
            sb.Append("</div>");
            ViewData["MenuInfoList"] = sb.ToString();
            ViewData["RoleId"] = Request["RoleId"];
            return View();
        }

        [HttpPost]
        public ActionResult SetMenuInfo(string msg)
        {
            string InfoList = Request["MenuListInfo"];
            string RoleId = Request["RoleId"];
            Business.Sys_Role Sys_Role = new Business.Sys_Role();
            if (Sys_Role.UpdateRoleMenuIdList(RoleId, InfoList))
            {
                Response.Write("<script>parent.layer.closeAll();</script>");
            }
            else
            {
                Response.Write("<script>parent.layer.closeAll();</script>");
            }

            return View();
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