using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYMVC.Models
{
	[System.ComponentModel.DataAnnotations.Schema.Table("AspNetUsers")]
    public class AspNetUsers
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }
}
