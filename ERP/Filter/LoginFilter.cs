using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP
{
    public class LoginFilter : Controller
    {

        /// <summary>
        /// 要过滤的控制器
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext) //Protected 只能被子类访问
        {
            if (System.Web.HttpContext.Current.Session["UsersId"] == null)
            {
                Response.Redirect("/Login/Index", true);
                Response.End();
            }
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="photourl"></param>
        protected void DeleteFlowerPhoto(string photourl)
        {
            string path = Server.MapPath("~") + photourl;
            //删除图片
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}