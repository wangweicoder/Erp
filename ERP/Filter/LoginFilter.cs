using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP
{
    public class LoginFilter : Controller
    {
        //要过滤的控制器     
        protected override void OnActionExecuting(ActionExecutingContext filterContext) //Protected 只能被子类访问
        {
            if (System.Web.HttpContext.Current.Session["UsersId"] == null)
            {
                Response.Redirect("/Login/Index", true);
                Response.End();
            }
        }
    }
}