using RV.Test.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RV.Test.Web.Models
{
    public class Widget : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Inventory { get; set; }
        public bool Melts { get; set; }
    }
}
