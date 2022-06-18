using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SYMVC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class DbCtxt : IdentityDbContext<ApplicationUser>
    {
		//public static DbCtxt Ctxt = Create();

        private DbCtxt(string connStr)
            : base(connStr, throwIfV1Schema: false)
        {
        }

        public static DbCtxt Create()
        {
			string connstr = System.Configuration.ConfigurationManager.ConnectionStrings["YieldDB"].ConnectionString;
			if (!connstr.Contains(";")) //encrypted, base64 blocks ';'
				connstr = Crypto.Decrypt(connstr);
			return new DbCtxt(connstr);
        }

		public System.Data.Entity.DbSet<SYMVC.Models.FAQ> FAQSet { get; set; }

		public System.Data.Entity.DbSet<SYMVC.Models.ContactUs> ContactUsSet { get; set; }

		public System.Data.Entity.DbSet<SYMVC.Models.Investment> InvestmentSet { get; set; }

		public System.Data.Entity.DbSet<SYMVC.Models.InvestmentHistory> InvestmentHistorySet { get; set; }

		public System.Data.Entity.DbSet<SYMVC.Models.UserAccount> UserAccountSet { get; set; }

		public System.Data.Entity.DbSet<SYMVC.Models.AccountInvestment> AccountInvestmentSet { get; set; }

		public System.Data.Entity.DbSet<SYMVC.Models.AccountTransaction> AccountTransactionSet { get; set; }

		public System.Data.Entity.DbSet<SYMVC.Models.TransactionType> TransactionsTypeSet { get; set; }

		public System.Data.Entity.DbSet<SYMVC.Models.AccountValueHistory> AccountValueHistorySet { get; set; } 
	}
}