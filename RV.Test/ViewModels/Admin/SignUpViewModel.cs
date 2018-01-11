using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RV.Test.Web.ViewModels.Admin
{
    public class SignUpViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
