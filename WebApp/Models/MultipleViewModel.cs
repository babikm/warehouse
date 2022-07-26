using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class MultipleViewModel
    {
        public IEnumerable<Characteristic> Characteristics { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Company> Companies { get; set; }
    }
}
