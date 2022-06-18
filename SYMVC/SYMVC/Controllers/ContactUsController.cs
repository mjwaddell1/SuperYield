using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SYMVC.Models;
using System.Configuration;

namespace SYMVC.Controllers
{
    public class ContactUsController : ControllerBase
    {
        private DbCtxt db = DbCtxt.Create();

		// GET: ContactUs
		public ActionResult Index()
        {
            try
            {
                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
                if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

                return View(db.ContactUsSet.Where(cu => !cu.Deleted).ToList());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // GET: ContactUs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,FirstName,LastName,Email,Phone,Title,Msg")] ContactUs contactUs)
        {
            try
            {
                if (ModelState.IsValid)
                {
					string user = "(Not logged in)"; //assume not logged in
					if (Util.UserLoggedIn(Session))
					{
						int acctId = int.Parse(Session["UserAccountId"].ToString());
						contactUs.UserId = acctId;
						UserAccount acct = db.UserAccountSet.Find(acctId);
						user = acctId + " (" + db.Users.Find(acct.UserId).Email + ")";
					}
					db.ContactUsSet.Add(contactUs);
                    db.SaveChanges();
					//send email
					string logo = ConfigurationManager.AppSettings["LogoImg"]; //http://67.211.213.19/Content/SYLogoTreeFull.png
					string fromAddr = ConfigurationManager.AppSettings["FromAddress"];
					string toAddr = ConfigurationManager.AppSettings["ToAddress"];

					string strOut = "<html><body>";
					strOut += "<h2>Contact Us - Message Received</h2>";
					strOut += "<table cellspacing='0' style='border:thin solid black'>";
					strOut += "<tr><td style='border: solid thin black;padding: 5px 10px;'>Account Id</td><td style='border: solid thin black;padding: 5px 10px;'>" + user + "</td></tr>";
					strOut += "<tr><td style='border: solid thin black;padding: 5px 10px;'>First Name</td><td style='border: solid thin black;padding: 5px 10px;'>" + contactUs.FirstName + "</td></tr>";
					strOut += "<tr><td style='border: solid thin black;padding: 5px 10px;'>Last Name</td><td style='border: solid thin black;padding: 5px 10px;'>" + contactUs.LastName + "</td></tr>";
					strOut += "<tr><td style='border: solid thin black;padding: 5px 10px;'>Email</td><td style='border: solid thin black;padding: 5px 10px;'>" + contactUs.Email + "</td></tr>";
					strOut += "<tr><td style='border: solid thin black;padding: 5px 10px;'>Phone</td><td style='border: solid thin black;padding: 5px 10px;'>" + contactUs.Phone + "</td></tr>";
					strOut += "<tr><td style='border: solid thin black;padding: 5px 10px;'>Msg Title</td><td style='border: solid thin black;padding: 5px 10px;'>" + contactUs.Title + "</td></tr>";
					strOut += "<tr><td style='border: solid thin black;padding: 5px 10px;'>Message</td><td style='border: solid thin black;padding: 5px 10px;'>" + contactUs.Msg + "</td></tr>";
					strOut += "</table>";
					strOut += "<br/><img src='" + logo + "' style='width:100px;padding-left:20px'/>";
					strOut += "</body></html>";

					Util.SendEmail("Contact Us Message", strOut);

					Session["UserMsg"] = "Your message has been sent. Thank you for contacting Super Yield.";
                    return RedirectToAction("Create");
                }

                return View(contactUs);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }
        // GET: ContactUs/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
                if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

                if (id == null)
                {
                    return ErrorView("Bad ID: " + id.Value);
                }
                ContactUs contactUs = db.ContactUsSet.Find(id);
                if (contactUs == null)
                {
                    return HttpNotFound();
                }
                return View(contactUs);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // POST: ContactUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ContactUs contactUs = db.ContactUsSet.Find(id);
				contactUs.Deleted = true;
                //db.ContactUs.Remove(contactUs);
                db.SaveChanges();
				Session["UserMsg"] = "Message deleted";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
