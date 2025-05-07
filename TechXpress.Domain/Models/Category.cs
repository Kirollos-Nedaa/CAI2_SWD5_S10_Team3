using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Category
    {
        public int Category_Id { get; set; }
        public string Name { get; set; }

        // Navigation property for the Product collection
        public ICollection<Product> Product { get; set; }
    }
}
