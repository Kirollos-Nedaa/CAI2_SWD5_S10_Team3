using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Cart
    {
        public int Cart_ID { get; set; }
        public int Customer_ID { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
