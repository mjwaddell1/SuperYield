using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SYMVC.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SYMVC.ViewModels
{
	public class AccountInvestmentVO
	{
		public Investment Inv; 
		public AccountInvestment AccInv;

		[DisplayName("Buy Amount")]
		public decimal BuyVal { get; set; }
		[DisplayName("Value")]
		public decimal CurVal { get; set; }
		[DisplayName("% Change")]
		public decimal PctChg { get; set; }
		public decimal BuyLimit { get; set; }
	}

}