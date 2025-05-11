using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Domain.Models;

namespace TechXpress.Domain.ViewModels
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
    }
}
