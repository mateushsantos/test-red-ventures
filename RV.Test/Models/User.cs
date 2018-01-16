using RV.Test.Infra;
using System.ComponentModel.DataAnnotations;

namespace RV.Test.Web.Models
{
    public class User : BaseEntity
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gravatar cannot be empty")]
        public string Gravatar { get; set; }
    }
}
