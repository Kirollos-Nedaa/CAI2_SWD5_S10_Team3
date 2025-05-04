using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Order_Item
    {
        public int Order_Item_Id { get; set; }
        public int Order_Id { get; set; }
        public int Product_Id { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        
        // Navigation property
        public Order Orders { get; set; }
        public Product Products { get; set; }
    }
}
