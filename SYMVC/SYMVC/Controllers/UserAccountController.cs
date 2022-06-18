using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SYMVC.Models;
using Microsoft.AspNet.Identity;


namespace SYMVC.Controllers
{
    public class UserAccountController : ControllerBase
    {
        private DbCtxt db = DbCtxt.Create();

		// GET: UserAccount
		public ActionResult Index()
        {
            try
            {
                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
                if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

                return View(db.UserAccountSet.Where(u => !u.Deleted).ToList());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        public ActionResult FundingInfo()
        {
            return View();
        }

        // GET: UserAccount/Create
        public ActionResult Create()
        {
            try
            {
                Log.Info("Create Acct (get)", Session);
                //this is to show existing data (if not empty), does NOT create account in DB
                if (Session["UserAccountId"] == null) //if logged in, should be 0
                    return Redirect(Url.Content("~/Account/Login"));

                string guid = User.Identity.GetUserId();
                if (guid == null || Session["UserAccountId"] == null)
                    return Redirect(Url.Content("~/Account/Login"));

                if (Session["UserAccountId"] != null && Session["UserAccountId"].ToString() != "0")
                    RedirectToAction("Edit", Session["UserAccountId"].ToString());
                UserAccount userAccount = new UserAccount() { UserId = guid };
                //UserAccount userAccount = new UserAccount() { UserId = Session["UserGuid"].ToString() };

                return View(userAccount);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // POST: UserAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,FirstName,MiddleName,LastName,Company,Addr1,Addr2,City,State,Zip,Country,Phone1,Phone2,PaymentMethod,BankRouting,BankAccount,CreateUser,CreateDate,UpdateUser,UpdateDate")] UserAccount userAccount)
        {
            try
            {
                Log.Info("Create Acct", Session);
                //save new account to DB

                if (ModelState.IsValid)
                {
                    userAccount.UserId = User.Identity.GetUserId(); //guid
                    userAccount.Email = db.Users.Find(userAccount.UserId).Email;
                    userAccount.Type = 1; //client user
                    userAccount.CreateDate = DateTime.Now;
                    Session["UserGuid"] = userAccount.UserId;
                    db.UserAccountSet.Add(userAccount);
                    db.SaveChanges();
                    UserAccount acct = db.UserAccountSet.Where(x => x.UserId == userAccount.UserId && !x.Deleted).FirstOrDefault();
                    Session["UserAccountId"] = acct == null ? 0 : acct.Id;

                    if (acct != null)
                    {
                        //Create settlement\cash investment
                        AccountInvestment ai = new AccountInvestment();
                        ai.AccountId = acct.Id;
                        ai.InvestmentId = 2;
                        ai.BuyAmt = 0;
                        ai.BuyDate = DateTime.Now;
                        ai.ForSale = false;
                        ai.CreateDate = DateTime.Now;
                        db.AccountInvestmentSet.Add(ai);
                        db.SaveChanges();
                    }

                    //return RedirectToAction("Edit", new { id = acct.Id});
					Session["UserMsg"] = "Update Successful";
                    //return RedirectToAction("Index", "Dashboard");
                }

                return View(userAccount);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // GET: UserAccount/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                Log.Info("Edit Acct (get): " + id, Session);

                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));

				id = int.Parse(Session["UserAccountId"].ToString());  //always edit current user

				if (id == null)
                {
					if (Session["UserAccountId"] == null)
						return ErrorView("UserAccount ID not set");
					else
						id = int.Parse(Session["UserAccountId"].ToString());
                }

                if (id == 0)
					return ErrorView("UserAccount ID = 0");
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                UserAccount userAccount = db.UserAccountSet.Find(id);
                if (userAccount == null)
                {
					return ErrorView("UserAccount (ID " + id + ") not found");
				}
                return View(userAccount);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // POST: UserAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,MiddleName,LastName,Company,Addr1,Addr2,City,State,Zip,Country,Phone1,Phone2,PaymentMethod,BankRouting,BankAccount,CreateUser,CreateDate,UpdateUser,UpdateDate,Type,Status,Email")] UserAccount userAccount)
        {
            try
            {
                Log.Info("Lock Acct: " + userAccount.Id, Session);
				//save edit data

				if (ModelState.IsValid)
				{
					userAccount.Id = int.Parse(Session["UserAccountId"].ToString()); //prevent update another user
					userAccount.UpdateDate = DateTime.Now;
					userAccount.UserId = User.Identity.GetUserId();
					//userAccount.Email = db.Users.Find(User.Identity.GetUserId()).Email;

					db.Entry(userAccount).State = EntityState.Modified;
					//ignore these props
					db.Entry(userAccount).Property(x => x.Type).IsModified = false;
					db.Entry(userAccount).Property(x => x.Status).IsModified = false;
					db.Entry(userAccount).Property(x => x.Email).IsModified = false;

					db.SaveChanges();

					Session["UserMsg"] = "Update Successful";
				}
                return View(userAccount);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        // GET: UserAccount/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
            if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

            if (id == null) return ErrorView("User Account ID Not Found");

            UserAccount userAccount = db.UserAccountSet.Find(id);

            if (userAccount == null) return ErrorView("User Account Not Found");

            return View(userAccount);
        }

        // POST: UserAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
                if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

                UserAccount userAccount = db.UserAccountSet.Find(id);
				userAccount.Deleted = true;
                //db.UserAccounts.Remove(userAccount);
                db.SaveChanges();

				Session["UserMsg"] = "Delete Successful";
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
