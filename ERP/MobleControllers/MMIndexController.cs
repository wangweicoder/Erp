using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.MobleControllers
{
    public class MMIndexController : MLoginFilterController
    {
        //
        // GET: /MMIndex/
        public ActionResult Index()
        {
            int id = (int)Session["UsersId"];
            Business.Sys_UserAdmin bus = new Business.Sys_UserAdmin();
            Model.UserAdmin model = bus.GetUserAdminByUserId(id);
            return View(model);
        }
	}
}