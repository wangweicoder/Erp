using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            HttpContext.Session.RemoveAll(); 
            return View();
           
        }
        public ActionResult LoginOut()
        {
            HttpContext.Session.RemoveAll();
            return Redirect("/Login/Index");
        }
        [HttpPost]
        public ActionResult Index(Model.UserAdmin UserAdmin)
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            UserAdmin.PassWord = Utility.ChangeText.md5(UserAdmin.PassWord);
            UserAdmin = Sys_UserAdmin.AdminLogin(UserAdmin);
            if (UserAdmin != null)
            {
                if (UserAdmin.IsEnable == 1)
                {
                    ViewBag.LoginError = "您的账号已经被禁止使用";
                    return View();
                }
                //登录成功
                Session["UsersId"] = UserAdmin.ID;
                Session["UserName"] = UserAdmin.UserName;
                Session["RealName"] = UserAdmin.RealName;
                Session["OwnedCompany"] = UserAdmin.OwnedCompany;
                //记录日志  跳转界面
                return RedirectToAction("Index", "Main");//跳转到首页。
            }
            ViewBag.LoginError = "账号或密码错误";
            return View();
        }


        public ActionResult Test() 
        {
            return View();
        }
	}
}