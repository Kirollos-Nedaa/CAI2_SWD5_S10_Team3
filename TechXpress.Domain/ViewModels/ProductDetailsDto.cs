using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class ProductDetailsDto
    {
        public int Product_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

        // Optional display values from navigation properties
        public int Category_Id { get; set; }
        public string CategoryName { get; set; }

        public int Brand_Id { get; set; }
        public string BrandName { get; set; }

        // You can also add calculated fields if needed
        public bool IsInStock => Quantity > 0;

        // If showing discount info
        public decimal? DiscountedPrice { get; set; }
        public bool IsWishlisted { get; set; }  // If logged-in user's wishlist info is available
    }
}
