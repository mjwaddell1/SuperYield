using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYMVC.Models;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace SYConsole
{
	class Program
	{
		readonly static int ConsoleUserId = 1; //admin for now

		static void Main(string[] args)
		{
			try
			{

				//List<int> lst = new List<int> { 2, 3, 10, 2, 4, 8, 1 };
				List<int> lst = new List<int> { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
				Console.WriteLine(maxDifference(lst));
				List<int> a = new List<int> { 5, 8, 4, 2, 6, 9, 7, 7, 7, 1 };
				List<int> m = new List<int> { 8, 2, 3, 6, 9, 5, 7, 3, 4, 6 };
				Console.WriteLine(winner(a, m, "Even"));

				//5-8+4-3+6-9+7-4

				List<float> ss = new List<float> { 2.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f };
				Console.WriteLine(efficientJanitor(ss));


				Console.ReadKey();
				return;

				/*
				log4net.Config.XmlConfigurator.Configure();

				Log.Info("==== START ====");

				//nightly batch
				DateTime RunDate = DateTime.Today;
				UpdateSettlementValues(RunDate);
				UpdateInvestmentValues(RunDate);
				UpdateAccountHistory(RunDate);
				BackupDB();
				EmailReport(RunDate);
				*/
			}
			catch (Exception ex)
			{
				Console.WriteLine(GetErrorStack(ex));
			}
			Log.Info("==== DONE ====");
			//Console.ReadLine(); //or Console.ReadKey();
		}

		public static int maxDifference(List<int> a)
		{
			if (a.Count == 1) return -1;
			int[] arr = a.ToArray();
			int max = -1;
			for (int x = 1; x < arr.Length; x++)
				for (int y = 0; y < x; y++)
					max = (arr[x] - arr[y] > max ? arr[x] - arr[y] : max);
			return max;
		}

		public static int efficientJanitor(List<float> weight)
		{
			//What the fuck?? I assume the bags are taken out IN ORDER???? That janitor is not efficient.
			int cnt = 0;
			float totWt = 0;
			foreach (float bg in weight)
			{
				if (totWt + bg > 3f)
				{
					cnt++;
					totWt = bg;
				}
				else
					totWt += bg;
			}
			return ++cnt;
		}

		public static string winner(List<int> andrea, List<int> maria, string s)
		{
			if (s == "Odd")	{ andrea.Remove(0); maria.Remove(0); }
			int scr = andrea.Where((ss, i) => i % 2 == 0).Zip(maria.Where((ss, i) => i % 2 == 0), (a, m) => (a - m)).Sum();
			if (scr == 0) return "Tie";
			Console.WriteLine(scr);
			return (scr > 0 ? "Andrea" : "Maria");			
		}

		public string FindNumberx (string SomeShit)
		{
			string[] strArr = SomeShit.Split('\r');
			string fd = strArr[strArr.Length - 1];
			strArr[0] = "";
			strArr[strArr.Length - 1] = "";
			int cnt = strArr.Where(x => x == fd).Count();
			if (cnt > 0) return "YES";
			return "NO";
		}

		//long[] lgArr = new long[strArr.Length - 1];
		//	for (int i = 1; i<lgArr.Length - 1; i++)
		//		lgArr[i - 1] = long.Parse(strArr[i]);

		static string findNumber(List<int> arr, int k)
		{
			if (arr.Where(x => x == k).Count() > 0)
				return "YES";
			else
				return "NO";

		}

		static List<int> oddNumbers(int l, int r)
		{
			List<int> lst = new List<int>();
			for (int x = l; x <= r; x++)
				if (x % 2 == 1) lst.Add(x);
			return lst;

		}


		public static void UpdateInvestmentValues(DateTime runDate)
		{
			Console.WriteLine("Begin UpdateInvestmentValues");
			Log.Info("UpdateInvestmentValues");
			try
			{
				var lstInv = SYContext.Instance.InvestmentSet.Where(i => i.Type != 1 && !i.Deleted).ToList();  //skip deposit, settlement
				foreach (var inv in lstInv)
				{
					int invCnt = SYContext.Instance.AccountInvestmentSet.Where(ai => ai.InvestmentId == inv.Id).Select(ai => ai.AccountId).Distinct().Count();
					Console.WriteLine(inv.Title + " " + invCnt);
					var lst = SYContext.Instance.AccountInvestmentSet.Where(ai => ai.InvestmentId == inv.Id).ToList();
					decimal totInvestment = 0;
					if (lst.Count > 0) totInvestment = lst.Select(ai => ai.Value.Value).Sum();
					DateTime runDateM1 = runDate.AddDays(-1);
					InvestmentHistory prevInv = SYContext.Instance.InvestmentHistorySet.Where(ih => ih.InvestmentId == inv.Id && ih.ValueDate == runDateM1).FirstOrDefault();
					InvestmentHistory curInv = SYContext.Instance.InvestmentHistorySet.Where(ih => ih.InvestmentId == inv.Id && ih.ValueDate == runDate).FirstOrDefault();
					if (curInv == null) //no entry for today
					{
						curInv = new InvestmentHistory
						{
							ValueDate = runDate,
							InvestmentId = inv.Id,
							InvestorCount = invCnt,
							CreateDate = DateTime.Now,
							CreateUser = ConsoleUserId,
							InvestmentValue = (prevInv == null ? 100 : prevInv.InvestmentValue.Value * 1.001m) //fake data
						};
						SYContext.Instance.InvestmentHistorySet.Add(curInv);
					}
					else //update entry
					{
						curInv.ValueDate = runDate;
						curInv.UpdateDate = DateTime.Now;
						curInv.UpdateUser = ConsoleUserId;
						curInv.InvestorCount = invCnt;
						curInv.InvestmentValue = (prevInv == null ? 100 : prevInv.InvestmentValue.Value * 1.001m); //fake data
					}
					SYContext.Instance.SaveChanges();
					UpdateAccountInvestments(inv, curInv);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(GetErrorStack(ex));
				Log.Error("UpdateInvestmentValues", ex);
			}
			Log.Info("Done UpdateInvestmentValues");
			Console.WriteLine("Done UpdateInvestmentValues");
		}

		public static void UpdateAccountInvestments(Investment inv, InvestmentHistory invHist)
		{
			try
			{
				//Console.WriteLine("Begin UpdateAccountInvestments");
				//Log.Info("UpdateAccountInvestments");
				//for each acct inv
				var lstAI = SYContext.Instance.AccountInvestmentSet.Where(ai => ai.InvestmentId == inv.Id && ai.BuyAmt != 0 && !ai.Deleted).ToList();
				foreach (var ai in lstAI)
				{
					decimal InvStartVal = SYContext.Instance.InvestmentHistorySet.First(h => h.InvestmentId == inv.Id && h.ValueDate == ai.BuyDate).InvestmentValue.Value;
					decimal InvCurVal = invHist.InvestmentValue.Value;
					//decimal InvCurVal = SYContext.Instance.InvestmentHistories.First(h => h.InvestmentId == x.InvestmentId && h.ValueDate == DateTime.Today).InvestmentValue.Value;
					ai.Value = InvCurVal / InvStartVal * ai.BuyAmt;
					ai.ValueDate = invHist.ValueDate;
					ai.UpdateDate = DateTime.Now;
					ai.UpdateUser = ConsoleUserId;
				}
				SYContext.Instance.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine(GetErrorStack(ex));
				Log.Error("UpdateAccountInvestments", ex);
			}
			//Log.Info("Done UpdateAccountInvestments");
			//Console.WriteLine("Done UpdateAccountInvestments");
		}

		public static void UpdateAccountHistory(DateTime runDate)
		{
			try
			{
				Console.WriteLine("Begin UpdateAccountHistory");
				Log.Info("UpdateAccountHistory");
				//account investment values already updated for runDate

				//user acct list
				var lstAcct = SYContext.Instance.UserAccountSet.Where(a => !a.Deleted).ToList();

				foreach (var acct in lstAcct)
				{
					decimal totInv = SYContext.Instance.AccountInvestmentSet.Where(i => i.AccountId == acct.Id && i.ValueDate == runDate && !i.Deleted).Select(i => i.Value.Value).Sum();
					AccountValueHistory avh = SYContext.Instance.AccountValueHistorySet.Where(h => h.AccountId == acct.Id && h.ValueDate == runDate && !h.Deleted).FirstOrDefault();
					if (avh == null) //expected, create new entry
					{
						avh = new AccountValueHistory()
						{ AccountId = acct.Id, Value = totInv, ValueDate = runDate, CreateUser = ConsoleUserId, CreateDate = DateTime.Now };
						SYContext.Instance.AccountValueHistorySet.Add(avh);
					}
					else // update existing entry
					{
						avh.Value = totInv;
						avh.UpdateUser = ConsoleUserId;
						avh.UpdateDate = DateTime.Now;
					}
					SYContext.Instance.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(GetErrorStack(ex));
				Log.Error("UpdateAccountHistory", ex);
			}
			Log.Info("Done UpdateAccountHistory");
			Console.WriteLine("Done UpdateAccountHistory");
		}

		public static void UpdateSettlementValues(DateTime runDate) //set current value of settlement\deposit accounts
		{
			try
			{
				Log.Info("UpdateSettlementValues");
				var lstAI = SYContext.Instance.AccountInvestmentSet.Where(ai => ai.InvestmentId < 3 && ai.BuyAmt != 0 && !ai.Deleted); //deposit / settlement
				foreach (var ai in lstAI)
				{
					ai.Value = ai.BuyAmt;
					ai.ValueDate = runDate;
					ai.UpdateDate = DateTime.Now;
					ai.UpdateUser = ConsoleUserId;
				}
				SYContext.Instance.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine(GetErrorStack(ex));
				Log.Error("UpdateSettlementValues", ex);
			}
			Log.Info("Done UpdateSettlementValues");
		}

		public static void BackupDB()
		{
			try
			{
				Log.Info("BackupDB");
				Console.WriteLine("Begin BackupDB");
				string path = ConfigurationManager.AppSettings["BackupFolder"];
				if (path.EndsWith("\\")) path = path.Substring(0, path.Length - 1); //strip \
				string filename = path + @"\DBYield_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
				string sql = @"BACKUP DATABASE YieldDB TO DISK = '" + filename + "'";
				Console.WriteLine("SQL = " + sql);
				Log.Info("SQL = " + sql);
				//string dbname = SYContext.Instance.Database.GetDbConnection().Database;
				int result = SYContext.Instance.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, sql);
				long filesize = new System.IO.FileInfo(filename).Length;
				Console.WriteLine("Done BackupDB (" + result + ") (" + filesize + " bytes)");
			}
			catch (Exception ex)
			{
				Console.WriteLine(GetErrorStack(ex));
				Log.Error("BackupDB", ex);
			}
			Log.Info("Done BackupDB");
		}

		public static void EmailReport(DateTime runDate)
		{
			try
			{
				Log.Info("EmailReport");
				Console.WriteLine("Begin EmailReport");
				string strOut = "<html><body>";
				////// Note - Style tag stripped out by gmail, must use inline styles
				strOut +=
				"<style>" +
				"table td, th\r\n" +
				"{\r\n" +
				"	border: solid thin black;\r\n" +
				"	padding: 5px 10px;\r\n" +
				"}\r\n" +
				"table th\r\n" +
				"{\r\n" +
				"	background-color:darkseagreen;\r\n" +
				"}\r\n" +
				"</style>\r\n";

				strOut += "<br/><br/><h2 style='color:forestgreen'>Daily Report for " + runDate + "</h2><br/>\r\n";
				//New users
				DateTime runDateM1 = runDate.AddDays(-1);
				var lstUANew = SYContext.Instance.UserAccountSet.Where(ua => ua.CreateDate > runDateM1).ToList();
				strOut += "<h3>-- New Users (" + lstUANew.Count + ")--</h3>\r\n";
				strOut += "<table cellspacing = '0' style = 'border:thin solid black;margin-bottom:20px'><tr><th> User Name </th><th> Email </th ></tr>\r\n";
				foreach (var usr in lstUANew)
					strOut += "<tr><td>" + usr.FirstName + " " + usr.LastName + "</td><td>" + usr.Email + "</td></tr>\r\n";
				if (lstUANew.Count == 0)
					strOut += "<tr><td colspan='2' style='padding:5px 10px;border:thin solid black'><b>No new users</b></td></tr>\r\n";
				strOut += "</table>\r\n";
				//User count
				int userCnt = SYContext.Instance.UserAccountSet.Where(u => u.Type == 1).Count(); //users only, no admin
				strOut += "<h3>-- Total Users: " + userCnt + " --</h3>\r\n";
				strOut += "<table cellspacing = '0' style = 'border:thin solid black;margin-bottom:20px'><tr><th>User Name</th><th>Email</th></tr>\r\n";
				var lstAllUsers = SYContext.Instance.UserAccountSet.Where(u => u.Type == 1 && !u.Deleted).ToList();
				foreach (var usr in lstAllUsers)
				{
					usr.Email = SYContext.Instance.AspNetUserSet.Where(u => u.Id == usr.UserId).First().Email;
					strOut += "<tr><td>" + usr.FirstName + " " + usr.LastName + "</td><td>" + usr.Email + "</td></tr>\r\n";
				}
				if (lstAllUsers.Count == 0)
					strOut += "<tr><td colspan='2' style='padding:5px 10px;border:thin solid black'><b>No users</b></td></tr>\r\n";
				strOut += "</table>\r\n";
				//Investments + Total amt in each investment (+ day change)
				strOut += "<h3>-- Investments --</h3>\r\n";
				strOut += "<table cellspacing='0' style='border:thin solid black;margin-bottom:20px'>\r\n";
				strOut += "<tr><th>Title</th><th>% Change (1 day)</th><th>Investors</th><th>Total Investment($)</th></tr>\r\n";
				var lstInv = SYContext.Instance.InvestmentSet.ToList();
				foreach (var inv in lstInv)
				{
					int invCnt = SYContext.Instance.AccountInvestmentSet.Where(ai => ai.InvestmentId == inv.Id && ai.AccountId != 1).Select(ai => ai.AccountId).Distinct().Count();
					//Total invested amount
					decimal totInv = 0;
					var lst = SYContext.Instance.AccountInvestmentSet.Where(ai => ai.InvestmentId == inv.Id && ai.AccountId != 1);
					if (lst.Count() > 0)
						totInv = lst.Select(ai => ai.Value.Value).Sum();

					decimal pctChg = 0;
					//DateTime runDateM1 = runDate.AddDays(-1);
					InvestmentHistory prevInv = SYContext.Instance.InvestmentHistorySet.Where(ih => ih.InvestmentId == inv.Id && ih.ValueDate == runDateM1).FirstOrDefault();
					InvestmentHistory curInv = SYContext.Instance.InvestmentHistorySet.Where(ih => ih.InvestmentId == inv.Id && ih.ValueDate == runDate).FirstOrDefault();
					if (prevInv != null)
						pctChg = (curInv.InvestmentValue.Value / prevInv.InvestmentValue.Value - 1.0m) * 100.0m;

					strOut += "<tr><td>"+inv.Title + "</td><td>" + pctChg + "</td><td>" + invCnt + "</td><td>" + totInv + "</td></tr>";
					//strOut += inv.Title + " : % Chg = " + pctChg + " : " + invCnt + " Investors : " + totInv + " Total Investment<br/>";
					//transactions last 24 hours ?
				}

				strOut += "</table>";
				string logo = ConfigurationManager.AppSettings["LogoImg"]; //http://67.211.213.19/Content/SYLogoTreeFull.png
				strOut += "<br/><img src='" + logo + "' style='width:100px;padding-left:20px'/>";
				strOut += "<br/><br/>";
				strOut += "</body></html>";

				//style tag stripped out, replace tags inline
				strOut = strOut.Replace("<td>", "<td style='border: solid thin black;padding: 5px 10px;'>");
				strOut = strOut.Replace("<th>", "<th style='border: solid thin black;padding: 5px 10px;background-color: darkseagreen;'>");

				SendEmail(strOut);

				Console.WriteLine(strOut);
			}
			catch (Exception ex)
			{
				Console.WriteLine(GetErrorStack(ex));
				Log.Error("EmailReport", ex);
			}
			Log.Info("Done EmailReport");
		}

		public static void SendEmail(string msg)
		{
			try
			{
				Log.Info("EmailReport");
				Console.WriteLine("Begin EmailReport");

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
				mail.Subject = "Super Yield Report";
				mail.Body = msg;
				client.Send(mail);
			}
			catch (Exception ex)
			{
				Console.WriteLine(GetErrorStack(ex));
				Log.Error("SendEmail", ex);
			}
			Log.Info("Done SendEmail");
		}

		public static string GetErrorStack(Exception ex)
		{
			string s = ex.Message;
			Exception inner = ex.InnerException;
			while (inner != null)
			{
				s += "\r\n-------\r\n" + inner.Message;
				inner = inner.InnerException;
			}
			return s;
		}
	}
}
