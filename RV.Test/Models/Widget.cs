using RV.Test.Infra;
using System.ComponentModel.DataAnnotations;

namespace RV.Test.Web.Models
{
    public class Widget : BaseEntity
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Color cannot be empty")]
        public string Color { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Price cannot be empty")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Inventory cannot be empty")]
        public int Inventory { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Melts cannot be empty")]
        public bool Melts { get; set; }
    }
}
