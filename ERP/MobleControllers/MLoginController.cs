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
                Session["RoleCode"] = null;
                Session["OpenId"] = null;               
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
                Session["OpenId"] = UserAdmin.OpenId;
                Session["UserName"] = UserAdmin.UserName;
                Session["RealName"] = UserAdmin.RealName;
                Session["RoleCode"] = UserAdmin.RoleCode;
                Sys_UserAdmin.UpdateOpenId(Session["OpenId"] != null ? Session["OpenId"].ToString() : "", UserAdmin.ID);
                //记录日志  跳转界面
                Business.Sys_UsersLoginLog Sys_Userlog = new Business.Sys_UsersLoginLog();
                Model.UsersLoginLog model = new Model.UsersLoginLog();
                model.UsersId = UserAdmin.ID.ToString();
                model.UserName = UserAdmin.UserName;
                model.RealName = UserAdmin.RealName;
                model.LoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                System.DateTime currentTime = DateTime.Now;
                model.Year = currentTime.Year.ToString();
                model.Month = currentTime.Month.ToString();
                model.Day = currentTime.Day.ToString();
                Sys_Userlog.InsertUsersLoginLog(model);
                return RedirectToAction("Index", "MMIndex");//跳转到首页。
            }
            ViewBag.LoginError = "账号或密码错误";
            return View();
        }
    }
}