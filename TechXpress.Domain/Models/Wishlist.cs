using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Wishlist
    {
        public int Wishlist_Id { get; set; }
        public int Customer_Id { get; set; }
        public int Product_Id { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}