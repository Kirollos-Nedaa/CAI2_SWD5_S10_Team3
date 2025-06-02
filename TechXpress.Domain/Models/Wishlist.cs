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
        public string Customer_Id { get; set; }
        public int Product_Id { get; set; }

        // Navigation properties
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }
}