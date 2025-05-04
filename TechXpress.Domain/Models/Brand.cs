using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
   public class Brand
    {
        public int Brand_Id { get; set; }
        public string Name { get; set; }
        public string imgUrl { get; set; }

        // Navigation property for the Product collection
        public ICollection<Product> Product { get; set; }
    }
}
