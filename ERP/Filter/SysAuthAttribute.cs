using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Filter
{
    public class SysAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var loginInfo = httpContext.Session["UsersId"];
            if (loginInfo == null)
            {              
                httpContext.Response.Redirect("~/Login/Index");
            }
            else 
            {
                TimeSpan SessTimeOut = new TimeSpan(0, 0, httpContext.Session.Timeout, 0, 0);
            }
            return loginInfo != null;
        }
    }
}