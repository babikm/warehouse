using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ProductViewModel
    {
        [DisplayName("Typ")]
        public string Type { get; set; }
        [DisplayName("Nazwa")]
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }
        [DisplayName("Waga")]
        public int Weight { get; set; }
    }
}
