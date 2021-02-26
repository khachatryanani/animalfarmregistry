using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RegistryWebApi.Configuration
{
    public class RegistryConfiguration
    {
        /// <summary>
        /// Configures Web API requests routing
        /// </summary>
        /// <param name="config">Http Configuration</param>
        public static void Register(HttpConfiguration config)
        {
            // Define attribute routing
            config.MapHttpAttributeRoutes();

            // Define uri rounting with default route template
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}