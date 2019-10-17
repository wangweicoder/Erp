using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            //将 SerializerSettings 重置为默认值 IgnoreSerializableAttribute = true,否则querymodel类因为使用了序列化属性而无法获取post传值
            //config.Formatters.JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings();

            //配置跨域
            var geduCors = new EnableCorsAttribute("*", "*", "GET,POST,PUT,DELETE,OPTIONS")
            {
                SupportsCredentials = true
            };
            config.EnableCors(geduCors);

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ERPApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //开启全局jwt验证
            //config.Filters.Add(new ApiAuthorizeAttribute());

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}