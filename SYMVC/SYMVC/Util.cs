using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SYMVC.Models;
using SYMVC.Controllers;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Reflection;

namespace SYMVC
{
	public class Util
	{
		public static decimal GetAcctInvCurVal(int id)
		{
			DbCtxt db = DbCtxt.Create();
			var x = db.AccountInvestmentSet.Find(id);
			return x.Value.Value; //set by night process
		}

		public static List<ViewModels.AccountInvestmentVO> GetInvestments(int accountId) //0 for all accounts
		{
			DbCtxt db = DbCtxt.Create();

			List<ViewModels.AccountInvestmentVO> lstAccInv = new List<ViewModels.AccountInvestmentVO>();
			foreach (var x in db.AccountInvestmentSet.Where(a => (a.AccountId == accountId || accountId == 0) && !a.Deleted).ToList())
			{
				ViewModels.AccountInvestmentVO ai = new ViewModels.AccountInvestmentVO(); //just for display, no db binding
				ai.AccInv = x;
				ai.Inv = db.InvestmentSet.Find(x.InvestmentId);

				//decimal InvStartVal = 0;
				if (ai.Inv.Type == 1) //for settlement\deposit acct (type=1), no history, price = buy value
				{
					ai.BuyVal = ai.CurVal = x.BuyAmt.Value;
					ai.PctChg = 0; //never changes
				}
				else
				{
					//InvStartVal = db.InvestmentHistorySet.First(h => h.InvestmentId == x.InvestmentId && h.ValueDate == x.BuyDate && !h.Deleted).InvestmentValue.Value;
					ai.BuyVal = x.BuyAmt.Value;
					ai.CurVal = x.Value.Value; //current value from console process
					ai.PctChg = ai.CurVal / ai.BuyVal;
					ai.PctChg -= 1; //for display
				}
				lstAccInv.Add(ai);
			}
			return lstAccInv.OrderBy(i => i.Inv.Id).ToList(); //settlement first
		}

		public static List<ViewModels.AccountInvestmentVO> GetInvestmentsForSale(int accountId, int skipAcct) //0 for all accounts
		{
			var iv = GetInvestments(accountId);
			var fs = iv.Where(i => i.AccInv.ForSale.Value && i.AccInv.AccountId != skipAcct);
			//calc 20% limit, set BuyLimit
			//for each 'for sale' investment
			//current sum of user same investment
			//(total acct value * .2) - cur inv = max inv buy amt
			//if max < 0, max = 0
			decimal acctTtl = iv.Select(i => i.CurVal).Sum(); //includes cash
			decimal pct20 = acctTtl * 0.2m;
			foreach (var ii in fs)
			{
				decimal sumInv = iv.Where(i => i.Inv.Id == ii.Inv.Id).Select(i => i.CurVal).Sum(); //amt already held
				ii.BuyLimit = pct20 - sumInv;
				if (ii.BuyLimit < 0) ii.BuyLimit = 0;
			}
			return fs.OrderBy(i => i.Inv.Id).ToList();
		}

		public static string GetUserGuid(HttpSessionStateBase ssn)
		{
			string UserGuid = (new AccountController()).SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();
			ssn["UserGuid"] = UserGuid;
			return UserGuid;
		}

		public static int GetUserAcctId(HttpSessionStateBase ssn)
		{
			string UserGuid = (new AccountController()).SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();
			DbCtxt db = DbCtxt.Create();
			UserAccount acct = db.UserAccountSet.Where(x => x.UserId == UserGuid).FirstOrDefault();
			ssn["UserAccountId"] = acct == null ? 0 : acct.Id;
			return acct == null ? 0 : acct.Id;
		}

		public static bool UserIsAdmin(HttpSessionStateBase ssn)  //if user is admin
		{
			if (ssn["UserType"] != null)
				return (ssn["UserType"].ToString() == ((int)AccountType.Admin).ToString());
			DbCtxt db = DbCtxt.Create();
			if (ssn["UserAccountId"] == null)
				return false;
			int acctId = int.Parse(ssn["UserAccountId"].ToString());
			if (db.UserAccountSet.Find(acctId) == null) //no account
				return false;
			int acctType = db.UserAccountSet.Find(acctId).Type;
			return acctType == (int)AccountType.Admin; //true if admin
		}

		public static bool UserIsInstitutional(int acctId)  //if user is Institutional
		{
			DbCtxt db = DbCtxt.Create();
			if (db.UserAccountSet.Find(acctId) == null) //no account
				return false;
			int acctType = db.UserAccountSet.Find(acctId).Type;
			return acctType == (int)AccountType.Institutional; //true if Institutional
		}

		public static bool UserIsInstitutional(HttpSessionStateBase ssn)  //if user is Institutional
		{
			if (ssn["UserType"] != null)
				return (ssn["UserType"].ToString() == ((int)AccountType.Institutional).ToString());
			DbCtxt db = DbCtxt.Create();
			if (ssn["UserAccountId"] == null)
				return false;
			int acctId = int.Parse(ssn["UserAccountId"].ToString());
			if (db.UserAccountSet.Find(acctId) == null) //no account
				return false;
			int acctType = db.UserAccountSet.Find(acctId).Type;
			return acctType == (int)AccountType.Institutional; //true if Institutional
		}

		public static bool UserLoggedIn(HttpSessionStateBase ssn)
		{
			return (ssn["UserAccountId"] != null);
		}

		public enum AccountType
		{
			Admin = 1, Institutional = 2, User = 3
		}

		public enum AcctStatus
		{
			Active = 1, Suspended, Locked, Closed
		}

		public enum InvType
		{
			Internal = 1, BusinessLoans, PreSettlement, RealEstate, Shipping, Finance, Insurance, Other
		}

		public enum TxType
		{
			Deposit = 1, DepositRequest, Withdraw, WithdrawRequest, Transfer, PostSell, CancelSell, Adjustment
		}

		public enum PymtMethod
		{
			BankDeposit = 1, Cash, GoldBullion, Bitcoin, CreditCard
		}

		public enum InvStatus
		{
			Setup = 1, Funding, ActiveOpen, ActiveClosed, Complete
		}

		public static string GetInvStatusString(int status)
		{
			//((Util.InvStatus)status).ToString(); //no good for ActiveOpen\Closed
			switch (status)
			{
				case 1: return "Setup";
				case 2: return "Funding";
				case 3: return "Active Open";
				case 4: return "Active Closed";
				case 5: return "Complete";
			}
			return "Unknown";
		}

		public static SelectList GetEnumSelect<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			var lst = new SelectList(Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(v => new SelectListItem
			{
				Text = v.ToString(),
				Value = ((int)(object)v).ToString()
			}).ToList(), "Value", "Text");
			return lst;
		}

		public static void SendEmail(string subject, string msg)
		{
			string fromAddr = ConfigurationManager.AppSettings["FromAddress"];
			string toAddr = ConfigurationManager.AppSettings["ToAddress"];
			string user = ConfigurationManager.AppSettings["RelayUser"];
			string pwd = ConfigurationManager.AppSettings["RelayPwd"];
			if (pwd.Length > 20) //encrypted
				pwd = Crypto.Decrypt(pwd);

			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(fromAddr);
			foreach (var address in toAddr.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
				mail.To.Add(address);
			//MailMessage mail = new MailMessage("Support@superyield.net", "mjwaddell@gmail.com");
			SmtpClient client = new SmtpClient();
			client.EnableSsl = true;
			client.Port = 587;
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			client.UseDefaultCredentials = false;
			//client.Credentials = new NetworkCredential("mike.waddell@superyield.net", "MWaddell1!");
			client.Credentials = new NetworkCredential(user, pwd);
			client.Host = "smtp.zoho.com";
			//mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
			mail.IsBodyHtml = true;
			mail.Subject = subject;
			mail.Body = msg;
			client.Send(mail);
		}

		public static string GetJSChartData(IEnumerable<SYMVC.ViewModels.AccountInvestmentVO> mdl) //allocation pie chart
		{
			Dictionary<string, decimal> dicInv = new Dictionary<string, decimal>();
			foreach (var i in mdl)
			{
				if (dicInv.ContainsKey(i.Inv.Title))
					dicInv[i.Inv.Title] += i.CurVal;
				else
					dicInv.Add(i.Inv.Title, i.CurVal);
			}
			string s = "['Asset', 'Value']";
			foreach (var i in dicInv)
			{
				s += ",['" + i.Key + "'," + i.Value + "]";
			}

			//string s = " ['Work', 11], ['Eat', 2], ['Commute', 2], ['Watch TV', 2], ['Sleep', 7]";
			return s;
		}

		public static string GetJSAcctValueData(HttpSessionStateBase ssn)
		{
			StringBuilder sb = new StringBuilder();
			DbCtxt db = DbCtxt.Create();
			int acctId = int.Parse(ssn["UserAccountId"].ToString());
			if (db.UserAccountSet.Find(acctId) == null) //no account
				return "";
			DateTime minDate = DateTime.Now.AddMonths(-6); //6 months max date range
			var acctHist = db.AccountValueHistorySet.Where(h => h.ValueDate >= minDate && h.AccountId == acctId && !h.Deleted).ToList();

			if (acctHist.Count == 0) return "['" + DateTime.Today.ToString("M/dd") + "',0.0]"; //single zero entry

			foreach (var h in acctHist)
			{
				sb.Append("['" + h.ValueDate.Value.ToString("M/dd") + "'," + h.Value + "],");
			}

			sb.Length--; //trim ,

			return sb.ToString();

			//         ['3/1', 37.8],
			//         ['4/1', 137.8],
			//         ['5/1', 237.8],
			//         ['6/1', 337.8],
			//         ['7/1', 437.8]
		}

		public static string ToCSV<theType>(List<theType> ListToConvert) where theType : class
		{
			List<List<string>> dataList = new List<List<string>>();

			bool first = true;
			//iterate through list items
			foreach (var item in ListToConvert)
			{
				//get properties and values 
				PropertyInfo[] props = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
				if (first)
				{
					List<string> itemVals = new List<string>();
					foreach (var prop in props)
						itemVals.Add(prop.Name);
					dataList.Add(itemVals);
				}

				List<string> itemValues = new List<string>();

				//iterate through properties
				foreach (var prop in props)
					itemValues.Add(prop.GetValue(item)?.ToString());

				dataList.Add(itemValues);
				first = false;
			}

			//flatten out lists and return results
			return string.Join(Environment.NewLine, dataList.Select(i => string.Join(",", i.Select(v => v?.ToString()))));
		}
	}

}
