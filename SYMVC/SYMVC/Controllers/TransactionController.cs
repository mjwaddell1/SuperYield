using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SYMVC.Models;
using System.Net;

namespace SYMVC.Controllers
{
    public class TransactionController : ControllerBase
    {
		private DbCtxt db = DbCtxt.Create();

		// GET: Transaction
		public ActionResult Index(int? id)
        {
            try
            {
                Log.Info("Transaction Index: " + id, Session);
                if (Session["UserAccountId"] == null)
                    return Redirect(Url.Content("~/Account/Login"));
                else
                    id = int.Parse(Session["UserAccountId"].ToString());

                if (id == 0) return ErrorView("Invalid User Account");

                UserAccount userAccount = db.UserAccountSet.Find(id);
                if (userAccount == null) return ErrorView("Account Not Found (ID " + id + ")");

                var lstAccTx = db.AccountTransactionSet.Where(tx => (tx.FromAccount == id || tx.ToAccount == id) && !tx.Deleted).ToList();

                SetNames(lstAccTx);

                return View(lstAccTx);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
                return ErrorView(ex.Message);
            }
        }

        public void SetNames(List<AccountTransaction> lstTx)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex.Message, Session, ex);
            }
        }

		public ActionResult Export() //csv
		{
			Log.Info("Export CSV - TC", Session);
			if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
			if (!Util.UserIsAdmin(Session)) return Redirect(Url.Content("~/Account/Login"));

			var lstAccTx = db.AccountTransactionSet.Where(tx => !tx.Deleted).ToList(); //all tx from all users

			SetNames(lstAccTx);  //investment names

			string csv = Util.ToCSV(lstAccTx);
			return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "AllTransactions.csv");
		}
	}
}
