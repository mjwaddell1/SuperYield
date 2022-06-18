using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SYMVC.Controllers
{
	public class HomeController : ControllerBase
	{
		public ActionResult Index()
		{
			return View();
		}

		public ViewResult Error(int? id)
		{
			if (id == 404)
			{
				Log.Error("404 Error [" + Request.UserHostAddress + " / " + Request.UserHostName + "] " + Request.Url, Session);
				return ErrorView("We cannot find the page you are looking for.");
			}
			return ErrorView("There has been an error in this request. If this error continues, please contact Super Yield.");
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
		/*
		public ActionResult PageNotFound(string[] segments)   //catchall route
		{
			ViewBag.ErrMsg = "You're Lost";
			return View("Error");
		}
		*/
	}
}