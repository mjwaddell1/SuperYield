using SYMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SYMVC.Controllers
{
    public class AdminController : ControllerBase
    {
		private DbCtxt db = DbCtxt.Create();
		// GET: Admin
		public ActionResult Index()
        {
            return View();
        }

		public ActionResult UserList()
		{
            try
            {
                Log.Info("User List", Session);
                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
                if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

                var accts = db.UserAccountSet.Where(ua => !ua.Deleted).ToList(); //not admin
                //var accts = db.UserAccountSet.Where(ua => ua.Type != (int)Util.AccountType.Admin && !ua.Deleted).ToList(); //not admin
                foreach (var usr in accts)
                {
                    usr.Email = db.Users.Find(usr.UserId).Email;
                    usr.StatusName = usr.Status == 1 ? "Active" : "Locked";
                }

                return View(accts);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

		//GET /Admin/ToggleInstitutional/id
		public ActionResult ToggleInstitutional(int? acctId)
		{
			try
			{
				Log.Info("ToggleInstitutional", Session);
				if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
				if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

				UserAccount ua = db.UserAccountSet.Find(acctId);
				ua.Type = 5 - ua.Type;  // 2/3
                db.SaveChanges();

				Log.Info("Type set to " + ua.Type + " " + (((Util.AccountType)ua.Type).ToString()), Session);

				return RedirectToAction("UserList");
				//return new EmptyResult();
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, Session, ex);
				return ErrorView(ex.Message);
			}

		}

        public ActionResult LockAccount(int? acctId) //lock\unlock acct
        {
            try
            {
                Log.Info("Lock Acct: " + acctId, Session);
                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
                if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));
                UserAccount acct = db.UserAccountSet.Find(acctId);
                acct.Status = 4 - acct.Status; //toggle 1/3
				Log.Info("LockAccount:New Status: " + acct.Status, Session);
                db.SaveChanges();
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

		public ActionResult LockAccountInv(int? acctId) //lock\unlock acct
		{
			try
			{
				Log.Info("Lock Acct: " + acctId, Session);
				if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
				if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));
				UserAccount acct = db.UserAccountSet.Find(acctId);
				acct.Status = 4 - acct.Status; //toggle 1/3
				Log.Info("LockAccountInv:New Status: " + acct.Status, Session);
				db.SaveChanges();
				return RedirectToAction("UserInvestments", new { acctId = acctId });
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, Session, ex);
				return ErrorView(ex.Message);
			}
		}

		public ActionResult UserInvestments(int? acctId) //lock\unlock acct
        {
            try
            {
                if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
                if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

                UserAccount acct = db.UserAccountSet.Find(acctId);
                var lstAccInv = Util.GetInvestments(acctId.Value);
                acct.Email = db.Users.Find(acct.UserId).Email;
                return View(Tuple.Create(lstAccInv, acct));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        public ActionResult AllTransactions()
		{
			Log.Info("All Tx", Session);
			if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
            if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

            var lstAccTx = db.AccountTransactionSet.Where(tx => !tx.Deleted).ToList(); //all tx from all users

			SetNames(lstAccTx);  //investment names

			return View(lstAccTx);
		}

		public void SetNames(List<AccountTransaction> lstTx)
		{
			foreach (var x in lstTx)
			{
				x.TypeName = db.TransactionsTypeSet.Find(x.Type).Title;
				var uaFrom = db.UserAccountSet.Find(x.FromAccount.Value);
				x.FromAccountName = uaFrom.FirstName + " " + uaFrom.LastName;
				if (x.ToAccount != null)
				{
					var uaTo = db.UserAccountSet.Find(x.ToAccount.Value);
					x.ToAccountName = uaTo.FirstName + " " + uaTo.LastName;
				}
				if (x.FromInvestment != null)
					x.FromInvestmentName = db.InvestmentSet.Find(x.FromInvestment)?.Title;
				if (x.ToInvestment != null)
					x.ToInvestmentName = db.InvestmentSet.Find(x.ToInvestment)?.Title;
			}
		}

		public ActionResult Export()
		{
			Log.Info("Export CSV - AC", Session);
			if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
			if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

			var lstAccTx = db.AccountTransactionSet.Where(tx => !tx.Deleted).ToList(); //all tx from all users

			SetNames(lstAccTx);  //investment names

			string csv = Util.ToCSV(lstAccTx);
			return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "AllTransactions.csv");
		}

        public ActionResult UpdateUserInvestment(int? id)
        {
            Log.Info("Update User Inv: " + id, Session);
            if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
            if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

            decimal amt = 0;
            if (!decimal.TryParse(Request["txtAdjAmt" + id] == "" ? "0" : Request["txtAdjAmt" + id], out amt))
            {
                Session["BS_ErrMsg"] = "Invalid Adjustment Amount (" + Request["txtAdjAmt" + id] + ")";
                return RedirectToAction("UserInvestments", new { acctId = id });
            }
            var ai = db.AccountInvestmentSet.Find(id);
            ai.BuyAmt = amt;

            //add tx record, for adjustment
            AccountTransaction tx = new AccountTransaction()
            { Type = (int)Util.TxType.Adjustment, Amount = amt, FromAccount = ai.AccountId, FromInvestment = ai.InvestmentId, CreateDate = DateTime.Now };
            db.AccountTransactionSet.Add(tx);

            db.SaveChanges();

			Session["UserMsg"] = "Update Successful";
            return RedirectToAction("UserInvestments", new { acctId = ai.AccountId });
        }

		public ActionResult GetLogFile()
		{
			Log.Info("Get Log File", Session);
			string fn = "";
			try
			{
				if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

				string lg = Log.Instance.GetLogFileName();

				string src = lg; // Server.MapPath("~/Logs/SY.log");
				fn = "SY_" + DateTime.Now.ToString("yyMMdd_HHmmssff") + ".log"; //temp file name
				string tgt = Server.MapPath("~/Logs/" + fn); //for IIS read access
				System.IO.File.Copy(src, tgt);
				return File(tgt, "text/plain", fn); //download
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, Session, ex);
				return ErrorView("Log File Not Found - " + fn);
			}
			//return File(tgt, System.Net.Mime.MediaTypeNames.Text.Plain);
		}
	}
}
