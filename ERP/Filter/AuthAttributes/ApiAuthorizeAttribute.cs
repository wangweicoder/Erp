using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using JWT;
using JWT.Serializers;

namespace ERP
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        //AuthorizeAttribute属性置于Action前，在调用Action前会进行验证。
        //AuthorizeAttribute属性置于Controller类前，在调用Controller中任何Action前会进行验证。
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var authHeader = from t in actionContext.Request.Headers where t.Key == "auth" select t.Value.FirstOrDefault();

            string tokenKey = authHeader.FirstOrDefault();

            TokenInfo json;
            bool result = TokenHelper.VaildateToken(tokenKey, out json);
            if (result)
            {
                actionContext.RequestContext.RouteData.Values.Add("auth", json);
            }            
            return result;
        }       
        //HandleUnauthorizedRequest函数则是在AuthorizeCore返回结果是false时会调用的函数。
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            var response = actionContext.Response ?? new HttpResponseMessage();
            response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            Dictionary<string, object> dic = new Dictionary<string, object>()
            {
                {"IsSuccess",false },
                {"Message","token认证失败,请重新申请" }
            };
            response.Content = new StringContent(JsonHelper.SerializeObject(dic), Encoding.UTF8, "application/json");            
        }
    }
}