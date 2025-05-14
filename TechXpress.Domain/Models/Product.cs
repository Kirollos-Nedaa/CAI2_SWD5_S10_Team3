using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Product
    {

        public int Product_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public int Category_Id { get; set; }
        public int Brand_Id { get; set; }

        // Navigation properties
        public Category Category { get; set; }
        public Brand Brand { get; set; }

        // Navigation properties for the related collections
        public ICollection<Cart_Items> Cart_Items { get; set; }
        public ICollection<Discount_Item> Discount_Items { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
    }
}
