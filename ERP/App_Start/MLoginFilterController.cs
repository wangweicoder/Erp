using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP
{
    public class MLoginFilterController : Controller
    {
        //要过滤的控制器     
        protected override void OnActionExecuting(ActionExecutingContext filterContext) //Protected 只能被子类访问
        {
            if (System.Web.HttpContext.Current.Session["OpenId"] == null)
            {
                if (choose_net(Request.Headers["User-Agent"]))
                {
                    string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + System.Configuration.ConfigurationManager.AppSettings["WxAppId"] + "&redirect_uri=" + System.Web.HttpUtility.UrlEncode("http://www.thuay.com/WxHelper/GetCode?id=" + Request["ArrangementId"] + "&way=" + Request["way"]) + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect ";
                    string gzhurl = "https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzI5OTEwODM2Ng==&scene=110#wechat_redirect";
                    Response.Redirect(url, true);
                    Response.End();
                }
            }
            if (System.Web.HttpContext.Current.Session["UsersId"] == null)
            {
                TimeSpan SessTimeOut = new TimeSpan(0, 0, System.Web.HttpContext.Current.Session.Timeout, 0, 0);
                Response.Redirect("/MLogin/Index", true);
                Response.End();
            }
        }

        public bool choose_net(string userAgent)
        {
            if (userAgent.IndexOf("MicroMessenger") > -1)// Nokia phones and emulators   
            {
                return true;
            }
            else
            {
                return false;
            }
        }  
	}
}