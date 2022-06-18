using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SYMVC
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",  //matches any route with 3 or less entries
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			/*
			//catchall - only works if more than 3 entries, else caught above
			routes.MapRoute(
				name: "CatchAll",
				url: "{*segments}", //any number of entries
				defaults: new { controller = "Home", action = "PageNotFound" }
			);
			*/
		}
	}
}
