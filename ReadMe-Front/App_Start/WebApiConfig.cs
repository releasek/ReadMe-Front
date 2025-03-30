using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ReadMe_Front
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 移除 XML 格式，強制 API 回傳 JSON
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
