using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.MobleControllers
{
    public class MLoginController : Controller
    {
        //
        // GET: /MLogin/
        public ActionResult Index()
        {
            string LoginOut = Request["LoginOut"];
            if (!string.IsNullOrEmpty(LoginOut))
            {
                Session["UsersId"] = null;
                Session["UserName"] = null;
                Session["RealName"] = null;
            }           
            return View();
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
                Session["RoleName"] = UserAdmin.RoleName;
                Sys_UserAdmin.UpdateOpenId(Session["OpenId"]!=null?Session["OpenId"].ToString():"", UserAdmin.ID);
                //记录日志  跳转界面
                return RedirectToAction("Index", "MMIndex");//跳转到首页。
            }
            ViewBag.LoginError = "账号或密码错误";
            return View();
        }
	}
}