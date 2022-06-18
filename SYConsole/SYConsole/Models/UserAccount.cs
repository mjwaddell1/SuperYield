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
	using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;


    [System.ComponentModel.DataAnnotations.Schema.Table("UserAccount")]
	public partial class UserAccount
    {
        public int Id { get; set; }
		public int Type { get; set; }  //  --1 = user, 2 = admin`
		public string UserId { get; set; }
		[DisplayName("First Name")]
        [MaxLength(128)]
        public string FirstName { get; set; }
		[DisplayName("Middle Name")]
        [MaxLength(128)]
        public string MiddleName { get; set; }
		[DisplayName("Last Name")]
        [MaxLength(128)]
        public string LastName { get; set; }
        [MaxLength(128)]
        public string Company { get; set; }
		[DisplayName("Address 1")]
        [MaxLength(128)]
        public string Addr1 { get; set; }
		[DisplayName("Address 2")]
        [MaxLength(128)]
        public string Addr2 { get; set; }
        [MaxLength(128)]
        public string City { get; set; }
        [MaxLength(2)]
        public string State { get; set; }
        [MaxLength(12)]
        public string Zip { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
		[DisplayName("Phone 1")]
        [MaxLength(15)]
        public string Phone1 { get; set; }
		[DisplayName("Phone 2")]
        [MaxLength(15)]
        public string Phone2 { get; set; }
		[DisplayName("Payment Method")]
        public Nullable<int> PaymentMethod { get; set; }
		[DisplayName("Bank Routing")]
        [MaxLength(15)]
        public string BankRouting { get; set; }
		[DisplayName("Bank Account")]
        [MaxLength(50)]
        public string BankAccount { get; set; }
		[DisplayName("Status")]
		public int Status { get; set; }
		[NotMapped]
		public string StatusName { get; set; }
		//[NotMapped]
		//public string Email; //get from user table
		public bool Deleted { get; set; }
		public Nullable<int> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}


/*
[Id] int identity primary key,
[Type] int default 1,  --1 = user, 2 = admin
[UserId] nvarchar(128),
[FirstName] varchar(128),
[MiddleName] varchar(128),
[LastName] varchar(128),
[Company] varchar(128),
[Addr1] varchar(128),
[Addr2] varchar(128),
[City] varchar(128),
[State] char(2),
[Zip] varchar(12),
[Country] varchar(50),
[Email] varchar(100),
[Phone1] varchar(15),
[Phone2] varchar(15),
[PaymentMethod] int foreign key references PaymentMethod(Id),
[BankRouting] varchar(15),
[BankAccount] varchar(50),
[Status] int default 1,
[CreateUser] int foreign key references UserAccount(Id),
[CreateDate] datetime default getdate(),
[UpdateUser] int foreign key references UserAccount(Id),
[UpdateDate] datetime
*/