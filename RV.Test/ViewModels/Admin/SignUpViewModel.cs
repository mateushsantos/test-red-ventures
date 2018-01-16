using System.ComponentModel.DataAnnotations;

namespace RV.Test.Web.ViewModels.Admin
{
    public class SignUpViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username cannot be empty")]
        public string Username { get; set; }
        [Compare("ConfirmPassword")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Confirmation cannot be empty")]
        public string ConfirmPassword { get; set; }
    }
}
