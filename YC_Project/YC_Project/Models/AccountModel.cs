using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PoTsen.Models
{
    public class AccountModel
    {
        [Display(Name = "Account")]
        [Required]
        public string Account { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}