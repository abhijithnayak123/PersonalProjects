using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TCF.Channel.Zeo.Web.Common;

namespace TCF.Channel.Zeo.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{

			routes.IgnoreRoute("_vti_bin/owssvr.dll");
			routes.IgnoreRoute("MSOffice/cltreq.asp");
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.Add(new Route("Scripts/Customer/{CustomerValidator}.js.axd", new NexxoScriptRoutingHandler()));

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			

			//This new Route is created to avoid QueryString # US1877 # TA4084
            routes.MapRoute(
                name: "InitiateCustomer",
                url: "CustomerSearch/{action}/{id}",
                defaults: new { Controller = "CustomerSearch", action = "IntitiateCustomerSession", id = UrlParameter.Optional }
                );

			routes.MapRoute(
				name: "Login",
                url: "{controller}/{action}/{channelPartner}",
                defaults: new { controller = "SSO", action = "Authenticate", channelPartner = UrlParameter.Optional }
            );
            
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{channelPartner}",
				defaults: new { controller = UrlParameter.Optional, action = UrlParameter.Optional, channelPartner = UrlParameter.Optional }
            );
		}


	}
}