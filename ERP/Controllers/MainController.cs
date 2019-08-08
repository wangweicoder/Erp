using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using ERP.Filter;

namespace ERP.Controllers
{
    [SysAuth]
    public class MainController : Controller
    {
        //
        // GET: /Main/       
        public ActionResult Index()
        {
            //读取菜单权限
            Business.Sys_Menu Sys_Menu = new Business.Sys_Menu();
            List<Model.Menu> MenuList=Sys_Menu.GetMenuList(Utility.ChangeText.GetUsersId());
            MenuList = MenuList.Where(x => x.Terminal.Trim() == "PC").ToList();
            List<Model.Menu> ParnetMenuList = MenuList.Where(x => x.Hierarchy == 1).ToList();
            List<Model.Menu> ChildrenMenuList = MenuList.Where(x => x.Hierarchy == 2).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var item in ParnetMenuList)
            {
                sb.Append("<li class=\"treeview\"><a><i class=\"fa fa-dashboard\"></i> <span>" + item.MenuName+ "</span><span class=\"pull-right-container\"><i class=\"fa fa-angle-left pull-right\"></i></span></a>");
                sb.Append("<ul class=\"treeview-menu\">");
                List<Model.Menu> ThisChildrenMenuList = ChildrenMenuList.Where(x => x.SuperiorMenuID == item.id).ToList();
                foreach (var items in ThisChildrenMenuList)
                {
                    sb.Append("<li class=\"active\"><a href='" + items.UrlPath + "' target='Content' ><i class=\"fa fa-clock-o\" ></i><span onclick=\"bianse(this)\" class=\"MenuSpan\">" + items.MenuName + "</span></a></li>");
                }
                sb.Append("</ul></li>");
            }
            ViewData["MenuListInfo"] = sb.ToString();
            return View();
        }

        public ActionResult ShowPhoto() 
        {
            string table = Request["table"];
            string id = Request["id"];
            if (table=="Flower")
            {
                Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
                ViewData["url"] = Sys_Flower.GetFlower(id).FlowerWatchPhoto;
            }
            if (table=="ProblemsAndSuggestions")
            {
                Business.Sys_ProblemsAndSuggestions Sys_ProblemsAndSuggestions = new Business.Sys_ProblemsAndSuggestions();
                ViewData["url"] = Sys_ProblemsAndSuggestions.GetModelById(id).PhotoList;
            }
            return View();
        }
	}
}