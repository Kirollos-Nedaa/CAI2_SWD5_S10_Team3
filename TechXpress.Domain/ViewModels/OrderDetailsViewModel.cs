using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderProductViewModel> Products { get; set; }
        public ShippingAddressViewModel ShippingAddress { get; set; }
    }

    public class OrderProductViewModel
    {
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class ShippingAddressViewModel
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Apartment { get; set; }
        public string PostCode { get; set; }
    }
}
