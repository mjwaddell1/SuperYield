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
    
	[System.ComponentModel.DataAnnotations.Schema.Table("TransactionType")]
    public partial class TransactionType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}