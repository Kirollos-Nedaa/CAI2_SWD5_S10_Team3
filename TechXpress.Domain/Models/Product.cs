using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Product
    {

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Inventory> Inventories { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }


        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int? SalesId { get; set; }
        public Discount_Sales? DiscountSales { get; set; }
    }
}
