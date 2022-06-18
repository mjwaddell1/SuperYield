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
				string ss = Console.ReadLine();

				Console.WriteLine(ss);
				return;


				//Console.WriteLine(Crypto.Encrypt("SYadmin#1"));
				//Console.WriteLine(Crypto.Encrypt(@"Data Source=.\SQLEXPRESS;Initial Catalog=YieldDB;User Id=sa;Password=SuperYield1;"));
				//Console.WriteLine(Crypto.Encrypt(@"Data Source=.\SQLEXPRESS;Initial Catalog=YieldDB;User Id=SYWeb;Password=SY0.!@#$%.Web;"));
				//Console.WriteLine(Crypto.Encrypt(@"Data Source=.\SQLEXPRESS;Initial Catalog=YieldDB;User Id=SYConsole;Password=SY0.!@#$%.Console;"));
				//Console.WriteLine(Crypto.Encrypt(@"SYadmn#1"));
				//Console.ReadKey();
				//return;

				log4net.Config.XmlConfigurator.Configure();

				Log.Info("==== START ====");

				//nightly batch
				DateTime RunDate = DateTime.Today;
				UpdateSettlementValues(RunDate);
				UpdateInvestmentValues(RunDate);
				UpdateAccountHistory(RunDate);
				BackupDB();
				EmailReport(RunDate);
			}
			catch (Exception ex)
			{
				Console.WriteLine(GetErrorStack(ex));
			}
			Log.Info("==== DONE ====");
			//Console.ReadLine(); //or Console.ReadKey();
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

					//fill in empty entries, assume every investment has entry
					DateTime dtMax = SYContext.Instance.InvestmentHistorySet.Where(ih => ih.InvestmentId == inv.Id).Select(i => i.ValueDate).Max().Value;
					if (dtMax < runDateM1) //fill in missing dates - flat pct
					{
						decimal val = SYContext.Instance.InvestmentHistorySet.Where(ih => ih.InvestmentId == inv.Id && ih.ValueDate == dtMax).FirstOrDefault().InvestmentValue.Value;
						for (DateTime dt = dtMax.AddDays(1); dt < runDate; dt = dt.AddDays(1))
						{
							var invNew = new InvestmentHistory
							{
								ValueDate = dt,
								InvestmentId = inv.Id,
								InvestorCount = invCnt,
								CreateDate = DateTime.Now,
								CreateUser = ConsoleUserId,
								InvestmentValue = val   //flat data
							};
							SYContext.Instance.InvestmentHistorySet.Add(invNew);
						}
						SYContext.Instance.SaveChanges();
					}

					InvestmentHistory prevInv = SYContext.Instance.InvestmentHistorySet.Where(ih => ih.InvestmentId == inv.Id && ih.ValueDate == runDateM1)?.FirstOrDefault();
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
							InvestmentValue = prevInv == null ? 100 : prevInv.InvestmentValue.Value * 1.001m //fake data, no rounding
						};
						SYContext.Instance.InvestmentHistorySet.Add(curInv);
					}
					else //update entry
					{
						curInv.ValueDate = runDate;
						curInv.UpdateDate = DateTime.Now;
						curInv.UpdateUser = ConsoleUserId;
						curInv.InvestorCount = invCnt;
						curInv.InvestmentValue = prevInv == null ? 100 : prevInv.InvestmentValue.Value * 1.001m; //fake data, no rounding
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

		public enum AccountType
		{
			Admin = 1, Institutional = 2, User = 3
		}

		public static void UpdateAccountInvestments(Investment inv, InvestmentHistory invHist)
		{
			try
			{
				var lstInst = SYContext.Instance.UserAccountSet.Where(ua => ua.Type == (int)AccountType.Institutional).Select(ua => ua.Id).ToList();
				//Console.WriteLine("Begin UpdateAccountInvestments");
				//Log.Info("UpdateAccountInvestments");
				//for each acct inv
				var lstAI = SYContext.Instance.AccountInvestmentSet.Where(ai => ai.InvestmentId == inv.Id && ai.BuyAmt != 0 && !ai.Deleted).ToList();
				foreach (var ai in lstAI)
				{
					decimal InvStartVal = SYContext.Instance.InvestmentHistorySet.First(h => h.InvestmentId == inv.Id && h.ValueDate == ai.BuyDate).InvestmentValue.Value;
					decimal InvCurVal = invHist.InvestmentValue.Value;
					//decimal InvCurVal = SYContext.Instance.InvestmentHistories.First(h => h.InvestmentId == x.InvestmentId && h.ValueDate == DateTime.Today).InvestmentValue.Value;
					ai.Value = Rnd2(InvCurVal / InvStartVal * ai.BuyAmt);  //round to penny for buy\sell
					ai.ValueDate = invHist.ValueDate;
					ai.UpdateDate = DateTime.Now;
					ai.UpdateUser = ConsoleUserId;
					if (lstInst.Contains(ai.AccountId.Value) && ai.ForSale.Value)
						ai.Discount = (decimal)((ai.Value - ai.BuyAmt) * .5m / ai.Value * 100); // 50% of profit
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
					strOut += "<tr><td>" + usr.FirstName + " " + usr.LastName + "</td><td>" + usr.Email + "</td><td>"+ (usr.Status == 1 ? "Active" : "Locked") + "</td></tr>\r\n";
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

		public static decimal Rnd2(decimal? nbr)
		{
			return Math.Round(nbr.Value, 2);
		}
	}
}
