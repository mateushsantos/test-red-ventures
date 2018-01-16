using RV.Test.Infra;
using System.ComponentModel.DataAnnotations;

namespace RV.Test.Web.Models
{
    public class Admin : BaseEntity
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username cannot be empty")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
    }
}
