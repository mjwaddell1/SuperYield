using SYMVC.Models;
using System.Data.Entity;
using System.Linq;

namespace SYConsole
{
    public class SYContext : DbContext
	{
        public DbSet<Investment> InvestmentSet { get; set; }
        public DbSet<AccountInvestment> AccountInvestmentSet { get; set; }
        public DbSet<AccountValueHistory> AccountValueHistorySet { get; set; }
        public DbSet<InvestmentHistory> InvestmentHistorySet { get; set; }
		public DbSet<UserAccount> UserAccountSet { get; set; }
        public DbSet<AspNetUsers> AspNetUserSet { get; set; }

        public static SYContext Instance = new SYContext();

		private SYContext()
		{
			string connstr = System.Configuration.ConfigurationManager.ConnectionStrings["YieldDB"].ConnectionString;
			if (!connstr.Contains(';')) //encrypted, base64 blocks ';'
				connstr = Crypto.Decrypt(connstr);
			this.Database.Connection.ConnectionString = connstr;
		}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var conn = System.Configuration.ConfigurationManager.ConnectionStrings["YieldDB"].ConnectionString;
        //    optionsBuilder.UseSqlServer(conn);
        //}
    }
}
