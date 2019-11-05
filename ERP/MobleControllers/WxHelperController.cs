using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.MobleControllers
{
    public class WxHelperController : Controller
    {
        //
        // GET: /WxHelper/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCode() 
        {
            try
            {                   
                string code = Request["code"];
                Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
                string OpenId = WxHelper.WxMain.Getopenid(code);
                Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByOpendId(OpenId);
                Session["OpenId"] = OpenId;
                if (UserAdmin != null && UserAdmin.RoleCode != "Tourist")
                {
                    Session["UsersId"] = UserAdmin.ID;
                    Session["UserName"] = UserAdmin.UserName;
                    Session["RealName"] = UserAdmin.RealName;
                    Session["RoleCode"] = UserAdmin.RoleCode;
                    //记录登录日志 
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
                    //if (Request["way"] == "Arrangement")
                    //{
                      //  string gzhurl = "https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=" + System.Configuration.ConfigurationManager.AppSettings["WxAppId"] + "&scene=110#wechat_redirect";
                       // string result=Utility.PostData.GetData(gzhurl);
                       // Utility.Log.WriteTextLog("微信自动关注公众号", "result", result , "gzhurl", gzhurl);
                        return RedirectToAction("GetArrangementInfo", "MMain", new { ArrangementId =Request["id"]});
                    //}
                   // else
                    //{
                      //  return RedirectToAction("Index", "MMIndex");
                    //}

                }
                else // if (Request["way"] == "Arrangement" )
                {
                    //string gzhurl = "https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=" + System.Configuration.ConfigurationManager.AppSettings["WxAppId"] + "&scene=110#wechat_redirect";
                    //string result = Utility.PostData.GetData(gzhurl);
                    //Utility.Log.WriteTextLog("微信自动关注公众号", "result", result, "gzhurl", gzhurl);
                    if (UserAdmin == null )
                    {
                        //System.Random Random = new System.Random();
                        //int Result = Random.Next(0, 9999);
                        //Business.Sys_Role Sys_Role = new Business.Sys_Role();
                        //Model.RoleInfo RoleInfo = Sys_Role.GetRoleInfoByRoleCode("Tourist");
                        ////写入一条记录 标识为游客
                        //Model.UserAdmin UserAdminTourist = new Model.UserAdmin();
                        //UserAdminTourist.UserName = "WeiXin_" + Result.ToString() + DateTime.Now.ToString("yyyy-MM-dd");
                        //UserAdminTourist.PassWord = Utility.ChangeText.md5("123456");
                        //UserAdminTourist.IsEnable = 0;
                        //UserAdminTourist.OpenId = OpenId;
                        //UserAdminTourist.RoleCode = RoleInfo.RoleCode;
                        //UserAdminTourist.RoleName = RoleInfo.RoleName;
                        //UserAdminTourist.RealName = "游客未知";
                        //Session["UsersId"] = Sys_UserAdmin.InsertUserAdminGetId(UserAdminTourist);
                        //Session["UserName"] = UserAdminTourist.UserName;
                        //Session["RealName"] = UserAdminTourist.RealName;
                        //Session["RoleCode"] = UserAdminTourist.RoleCode;
                        ////记录登录日志 
                        //Business.Sys_UsersLoginLog Sys_Userlog = new Business.Sys_UsersLoginLog();
                        //Model.UsersLoginLog model = new Model.UsersLoginLog();
                        //model.UsersId = UserAdmin.ID.ToString();
                        //model.UserName = UserAdmin.UserName;
                        //model.RealName = UserAdmin.RealName;
                        //model.LoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //System.DateTime currentTime = DateTime.Now;
                        //model.Year = currentTime.Year.ToString();
                        //model.Month = currentTime.Month.ToString();
                        //model.Day = currentTime.Day.ToString();
                        //Sys_Userlog.InsertUsersLoginLog(model);
                        //return RedirectToAction("GetArrangementInfo", "MMain", new { ArrangementId = Request["id"]});
                        return RedirectToAction("Index", "MLogin", new { ArrangementId = Request["id"] });
                    }
                    //if (UserAdmin.RoleCode=="Tourist")
                    //{
                    //    Session["UsersId"] = UserAdmin.ID;
                    //    Session["UserName"] = UserAdmin.UserName;
                    //    Session["RealName"] = UserAdmin.RealName; 
                    //    Session["RoleCode"] = UserAdmin.RoleCode;
                    //    //记录登录日志 
                    //    Business.Sys_UsersLoginLog Sys_Userlog = new Business.Sys_UsersLoginLog();
                    //    Model.UsersLoginLog model = new Model.UsersLoginLog();
                    //    model.UsersId = UserAdmin.ID.ToString();
                    //    model.UserName = UserAdmin.UserName;
                    //    model.RealName = UserAdmin.RealName;
                    //    model.LoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //    System.DateTime currentTime = DateTime.Now;
                    //    model.Year = currentTime.Year.ToString();
                    //    model.Month = currentTime.Month.ToString();
                    //    model.Day = currentTime.Day.ToString();
                    //    Sys_Userlog.InsertUsersLoginLog(model);
                    //    return RedirectToAction("GetArrangementInfo", "MMain", new { ArrangementId = Request["id"] });
                    //}
                }
                return RedirectToAction("Index", "MLogin"); 
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog("微信自动登陆异常", "", "", "", ex.ToString());
                return null;
            }
                    
        }
	}
}