//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SYMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [System.ComponentModel.DataAnnotations.Schema.Table("AccountInvestment")]
    public partial class AccountInvestment
    {
        public int Id { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> InvestmentId { get; set; }
        [DisplayName("Purchase Amount")]
        public Nullable<decimal> BuyAmt { get; set; }
        [DisplayName("Purchase Date")]
        public Nullable<System.DateTime> BuyDate { get; set; }
        [DisplayName("Sell Date")]
        public Nullable<System.DateTime> SellDate { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<decimal> Discount { get; set; }
		[DisplayName("Value Date")]
        public Nullable<System.DateTime> ValueDate { get; set; }
		[DisplayName("Lock End Date")]
        public Nullable<System.DateTime> LockStartDate { get; set; }
        [DisplayName("For Sale")]
        public Nullable<bool> ForSale { get; set; }
		public bool Deleted { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        public virtual Investment Investment { get; set; }
    }
}