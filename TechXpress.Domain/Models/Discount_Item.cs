using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Discount_Item
    {
        public int Discount_Item_Id { get; set; }
        public int Product_Id { get; set; }
        public int Discount_Id { get; set; }

        // Navigation properties
        public Discount Discount { get; set; }
        public Product Product { get; set; }
    }
}
