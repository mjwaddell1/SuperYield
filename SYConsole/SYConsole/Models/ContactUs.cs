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
	using System.ComponentModel.DataAnnotations;

	public partial class ContactUs
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
		[DisplayName("First Name")]
        public string FirstName { get; set; }
		[DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
		[DisplayName("Subject")]
        public string Title { get; set; }
		[DisplayName("Message")]
		[DataType(DataType.MultilineText)]
		public string Msg { get; set; }
		public bool Deleted { get; set; }
	}
}