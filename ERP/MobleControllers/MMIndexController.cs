using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.MobleControllers
{
    public class MMIndexController : MLoginFilterController
    {
        [OutputCache(Duration = 20, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            int id = (int)Session["UsersId"];
            Business.Sys_UserAdmin bus = new Business.Sys_UserAdmin();
            Model.UserAdmin model = bus.GetUserAdminByUserId(id);
            ViewData["FlowerActive"] = GetList();
            return View(model);
        }
        private List<Model.FlowerActive> GetList()
        {            
            Business.Sys_FlowerActive Sys_FlowerActive = new Business.Sys_FlowerActive();
            return Sys_FlowerActive.FlowerActiveList(1, 1, "");
        }
	}
}