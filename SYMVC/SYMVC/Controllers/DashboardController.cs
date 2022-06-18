using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SYMVC.Models;
using System.Net;

namespace SYMVC.Controllers
{
    public class DashboardController : ControllerBase
    {
		private DbCtxt db = DbCtxt.Create();

		// GET: Dashboard
		public ActionResult Index(int? id)
        {
            try { 
			//if (id == null)
			//{
				if (Session["UserAccountId"] == null)
				    return Redirect(Url.Content("~/Account/Login"));
				else
					id = int.Parse(Session["UserAccountId"].ToString());
			//}

			Log.Info("Dashboard: " + id, Session);
			if (id == 0)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			UserAccount userAccount = db.UserAccountSet.Find(id);
			if (userAccount == null)
			{
				return HttpNotFound();
			}

			var lstAccInv = Util.GetInvestments(id.Value);

			return View(lstAccInv);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }
    }
}
