using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Orders
    {
        public int Order_Id { get; set; }
        public int Customer_Id { get; set; }
        public DateTime Order_Date { get; set; }
        public decimal Total_Amount { get; set; }
        public int Shipping_Address_Id { get; set; }
        public string Order_Status { get; set; }

        // Navigation property
        public Customer Customer { get; set; }
        public Address Address { get; set; }
    }
}
