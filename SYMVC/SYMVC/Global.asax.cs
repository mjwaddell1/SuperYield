using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SYMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
			log4net.Config.XmlConfigurator.Configure();
			AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

		protected void Application_Error()
		{
			var ex = Server.GetLastError();
			Log.Error("Global Error ["+ Request.UserHostAddress + " / " + Request.UserHostName +"]", null, ex);
		}
	}
}
