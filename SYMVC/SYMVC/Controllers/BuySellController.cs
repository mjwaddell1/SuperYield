using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SYMVC.Models;

namespace SYMVC.Controllers
{
	public class BuySellController : ControllerBase
	{
		private DbCtxt db = DbCtxt.Create();
		public string ErrMsg = "";


		// GET: BuySell
		public ActionResult Index(int? acctId)
		{
			try
			{
				Log.Info("BuySell Index", Session);
				if (Session["UserAccountId"] == null)
					return Redirect(Url.Content("~/Account/Login"));
				else
					acctId = int.Parse(Session["UserAccountId"].ToString());

				if (acctId == 0)
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

				UserAccount userAccount = db.UserAccountSet.Find(acctId);
				if (userAccount == null)
					return HttpNotFound();

				var lstAccInv = Util.GetInvestments(acctId.Value);
				var lstInvForSale = Util.GetInvestmentsForSale(0, acctId.Value); //all inv for sale

				return View(Tuple.Create(lstAccInv, lstInvForSale));
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, Session, ex);
				return ErrorView(ex.Message);
			}
		}

		// POST: BuySell/Sell/5
		public ActionResult Sell(int id)
		{
			try
			{
				Log.Info("BuySell Sell", Session);
				if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
				int acctId = int.Parse(Session["UserAccountId"].ToString());

				UserAccount acct = db.UserAccountSet.Find(acctId);
				if (acct.Status == (int)Util.AcctStatus.Locked)
				{
					Session["ErrMsg"] = "Cannot complete action. Your account is locked. Please contact Super Yield support.";
					return RedirectToAction("Index");
				}

				Log.Info("PostSell: " + id, Session);

				if (Request["prfx"] == null)  // LG  SM
				{
					Session["ErrMsg"] = "Cannot process request. Request missing prfx.)";
					return RedirectToAction("Index");
				}

				string fldSellAmt = ""; //sell amt field name
				string fldDiscount = "";  // discount field name

				fldSellAmt = "txtSellAmt" + Request["prfx"] + id;
				fldDiscount = "txtDiscount" + Request["prfx"] + id;

				decimal amt = 0;
				if (!decimal.TryParse(Request[fldSellAmt] == "" ? "0" : Request[fldSellAmt], out amt))
				{
					Session["ErrMsg"] = "Invalid Sell Amount ($ " + Request[fldSellAmt] + ")";
					return RedirectToAction("Index");
				}
				if (amt < 0)
				{
					Session["ErrMsg"] = "Invalid Sell Amount ($ " + amt + ")";
					return RedirectToAction("Index");
				}
				decimal disc = 0;
				var ai = db.AccountInvestmentSet.Find(id);
				if (Util.UserIsInstitutional(Session))
				{
					disc = (decimal)((ai.Value - ai.BuyAmt) * .5m / ai.Value * 100); // 50% of profit
				}
				else
				{
					if (!decimal.TryParse(Request[fldDiscount] == "" ? "0" : Request[fldDiscount], out disc))
					{
						Session["ErrMsg"] = "Invalid Discount Amount (" + Request[fldDiscount] + " %)";
						return RedirectToAction("Index");
					}
					if (disc > 100 || disc < 0)
					{
						Session["ErrMsg"] = "Invalid Discount Amount (" + disc + " %)";
						return RedirectToAction("Index");
					}
				}
				if (ai.AccountId != acctId)
				{
					Session["ErrMsg"] = "Invalid Investment";
					return RedirectToAction("Index");
				}
				decimal curVal = Util.GetAcctInvCurVal(id);
				amt = Math.Abs(Rnd2(amt));
				if (amt == 0)
					amt = curVal; //sell all
				if (amt > curVal)
				{
					Session["ErrMsg"] = "Sell amount ($ " + amt + ") is greater than available investment ($ " + ai.Value.Value + ")";
					return RedirectToAction("Index");
				}
				if (curVal - amt < 1) //can't leave less than a dollar
					amt = curVal; //buy all
				if (amt < curVal) //must split investment
				{
					decimal pctSell = amt / curVal;
					AccountInvestment aiSell = new AccountInvestment
					{ AccountId = ai.AccountId, BuyAmt = Rnd2(ai.BuyAmt * pctSell), Discount = disc, InvestmentId = ai.InvestmentId, BuyDate = ai.BuyDate, Value = amt, ValueDate = ai.ValueDate, ForSale = true };
					aiSell.CreateUser = acctId;
					aiSell.CreateDate = DateTime.Now;

					db.AccountInvestmentSet.Add(aiSell);
					ai.BuyAmt -= aiSell.BuyAmt;
					ai.Value -= aiSell.Value;
				}
				else // sell all
				{
					AccountInvestment aix = db.AccountInvestmentSet.Find(id);
					aix.ForSale = true;
					aix.Discount = disc;
					aix.UpdateUser = acctId;
					aix.UpdateDate = DateTime.Now;
				}

				//add tx record, for sale
				AccountTransaction tx = new AccountTransaction()
				{ Type = (int)Util.TxType.PostSell, Amount = amt, Discount = disc, FromAccount = ai.AccountId, FromInvestment = ai.InvestmentId, CreateDate = DateTime.Now };
				db.AccountTransactionSet.Add(tx);

				db.SaveChanges();
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, Session, ex);
				return ErrorView(ex.Message);
			}
		}

		// GET: BuySell/Cancel/5
		public ActionResult Cancel(int id) //cancel sale
		{
			try
			{
				Log.Info("BuySell Cancel", Session);
				if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
				Log.Info("CancelSell: " + id, Session);
				var ai = db.AccountInvestmentSet.Find(id);
				int acctId = int.Parse(Session["UserAccountId"].ToString());

				UserAccount acct = db.UserAccountSet.Find(acctId);
				if (acct.Status == (int)Util.AcctStatus.Locked)
				{
					Session["ErrMsg"] = "Cannot complete action. Your account is locked. Please contact Super Yield support.";
					return RedirectToAction("Index");
				}

				if (!ai.ForSale.Value)
				{
					Session["ErrMsg"] = "Cannot cancel sale. Investment has been bought.";
					return RedirectToAction("Index");
				}
				//find matching investment to re-merge
				var prevAI = db.AccountInvestmentSet.Where(i => i.InvestmentId == ai.InvestmentId && i.BuyDate == ai.BuyDate && i.AccountId == ai.AccountId && i.ForSale.Value == false).FirstOrDefault();
				if (prevAI == null)
				{
					ai.ForSale = false; //no merge, just reverse sale
				}
				else
				{
					prevAI.BuyAmt += ai.BuyAmt;
					prevAI.Value += ai.Value;
					//db.AccountInvestments.Remove(ai);
					ai.Deleted = true;
				}

				ai.UpdateUser = acctId;
				ai.UpdateDate = DateTime.Now;

				decimal curVal = Util.GetAcctInvCurVal(id);

				//add tx record, cancel sell
				AccountTransaction tx = new AccountTransaction()
				{ Type = (int)Util.TxType.CancelSell, Amount = curVal, FromAccount = ai.AccountId, FromInvestment = ai.InvestmentId, CreateDate = DateTime.Now };
				db.AccountTransactionSet.Add(tx);

				db.SaveChanges();
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, Session, ex);
				return ErrorView(ex.Message);
			}
		}

		// GET: BuySell/Sell/5
		public ActionResult Buy(int id)
		{
			try
			{
				Log.Info("BuySell Buy", Session);
				if (Session["UserAccountId"] == null) return Redirect(Url.Content("~/Account/Login"));
				Log.Info("Buy: " + id, Session);
				int acctId = int.Parse(Session["UserAccountId"].ToString());

				string prfx = "";
				if (Request["prfx"] == null)  // LG  SM
				{
					Session["ErrMsg"] = "Cannot process request. Request missing prfx.)";
					return RedirectToAction("Index");
				}

				prfx = Request["prfx"];

				UserAccount acct = db.UserAccountSet.Find(acctId);
				if (acct.Status == (int)Util.AcctStatus.Locked)
				{
					Session["ErrMsg"] = "Cannot complete action. Your account is locked. Please contact Super Yield support.";
					return RedirectToAction("Index");
				}

				decimal enteredAmt = 0;
				//decimal valueAmt = 0; //without discount
				if (!decimal.TryParse(Request["txtBuyAmt" + prfx + id] == "" ? "0" : Request["txtBuyAmt" + prfx + id], out enteredAmt))
				{
					Session["ErrMsg"] = "Invalid Buy Amount ($ " + Request["txtBuyAmt" + prfx + id] + ")";
					return RedirectToAction("Index");
				}
				enteredAmt = Math.Abs(enteredAmt);
				var ai = db.AccountInvestmentSet.Find(id);
				if (!ai.ForSale.Value)
				{
					Session["ErrMsg"] = "Cannot purchase investment. Investment is not longer for sale.";
					return RedirectToAction("Index");
				}
				decimal fullVal = Util.GetAcctInvCurVal(id); //full value if buying all
				decimal costAll = fullVal; //price for buyer, if buying all 0% discount
				if (ai.Discount.Value > 0) //calc discount
					costAll = Rnd2(fullVal * (100m - ai.Discount.Value) / 100m); //price after discount, if buying all
				int fromAcctId = ai.AccountId.Value;
				if (enteredAmt == 0)
					enteredAmt = costAll; //buy all
				//get settlement\cash amount
				AccountInvestment cashAcctSeller = db.AccountInvestmentSet.Where(i => i.AccountId == ai.AccountId && i.InvestmentId == 2 && !i.Deleted).First();
				AccountInvestment cashAcctBuyer = db.AccountInvestmentSet.Where(i => i.AccountId == acctId && i.InvestmentId == 2 && !i.Deleted).FirstOrDefault();
				if (cashAcctSeller == null)
				{
					Session["ErrMsg"] = "Seller has no settlement account. He cannot sell investments.";
					return RedirectToAction("Index");
				}
				if (cashAcctBuyer == null)
				{
					Session["ErrMsg"] = "You have no settlement account. You cannot purchase investments.";
					return RedirectToAction("Index");
				}

				decimal origAmtEntered = 0;
				if (enteredAmt > costAll)
				{
					origAmtEntered = enteredAmt; //for msg 
					enteredAmt = costAll; //buy all
				}
				if (costAll - enteredAmt < 1) //can't leave less than a dollar
					enteredAmt = costAll; //buy all
				//if (amt > curVal)
				//{
				//	ViewBag.ErrorMessage = "Buy amount is greater than available amount";
				//	return RedirectToAction("Index");
				//	//return Content("Sell Amt > Cur Val");
				//}
				if (enteredAmt > cashAcctBuyer.BuyAmt.Value) //cash available of buyer
				{
					Session["ErrMsg"] = "Buy amount ($ " + enteredAmt + ") is greater than available cash ($ " + cashAcctBuyer.BuyAmt.Value + ")";
					return RedirectToAction("Index");
				}

				decimal pctSell = 1; //sell all

				Investment inv = db.InvestmentSet.Find(ai.InvestmentId);

				decimal costPortion = costAll; //default buy all
				decimal valPortion = fullVal;  //will include discount

				if (enteredAmt < costAll) //must split investment
				{
					pctSell = enteredAmt / costAll;
					costPortion = enteredAmt; //  Rnd2(costAll * pctSell); //with discount
					valPortion = Rnd2(fullVal * pctSell); //full value
					AccountInvestment aiBuy = new AccountInvestment
					{ AccountId = acctId, BuyAmt = costPortion, Value = costPortion, Discount = 0, ValueDate = ai.ValueDate, InvestmentId = ai.InvestmentId, BuyDate = DateTime.Today, CreateDate = DateTime.Now, ForSale = false };
					ai.BuyAmt *= 1 - pctSell; //remaining for seller
					ai.Value *= 1 - pctSell; //remaining for seller
					aiBuy.CreateUser = acctId;
					aiBuy.CreateDate = DateTime.Now;
					db.AccountInvestmentSet.Add(aiBuy);
					if (!aiBuy.LockStartDate.HasValue && inv.Type > 1)
						aiBuy.LockStartDate = DateTime.Today; //begin lock period
				}
				else // buy all, pctSell = 1
				{
					ai.AccountId = acctId; //just change user
					ai.ForSale = false;
					ai.BuyAmt = ai.Value; //no discount
					ai.BuyDate = DateTime.Today;
					if (!ai.LockStartDate.HasValue && inv.Type > 1)
						ai.LockStartDate = DateTime.Today; //begin lock period
					ai.UpdateUser = acctId;
					ai.UpdateDate = DateTime.Now;
				}


				//No merge needed, different buy dates

				//subtract cash from buyer settlement acct
				cashAcctBuyer.BuyAmt -= enteredAmt;
				//add cash from seller settlement acct
				cashAcctSeller.BuyAmt += enteredAmt;

				//add tx record, transfer
				AccountTransaction tx = new AccountTransaction()
				{ Type = (int)Util.TxType.Transfer, Amount = valPortion, Cost = costPortion, Discount = ai.Discount, FromAccount = fromAcctId, FromInvestment = ai.InvestmentId, ToAccount = acctId, CreateDate = DateTime.Now };
				db.AccountTransactionSet.Add(tx);

				db.SaveChanges();
				Session["UserMsg"] = "Purchase Successful" + (origAmtEntered > 0 ? " - Amount entered (" + origAmtEntered + ") was greater then available investment amount (" + enteredAmt + ") so full amount was purchased" : "");

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, Session, ex);
				return ErrorView(ex.Message);
			}
		}
		decimal Rnd2(decimal? nbr)
		{
			return Math.Round(nbr.Value, 2);
		}
	}
}
