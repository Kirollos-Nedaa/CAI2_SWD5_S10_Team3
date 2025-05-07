using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Cart_Items
    {
        public int Cart_Item_Id { get; set; }
        public int Cart_Id { get; set; }
        public int Product_Id { get; set; }
        public int Quantity { get; set; }

        // Navigation property
        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
