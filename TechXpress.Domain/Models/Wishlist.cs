using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Wishlist
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}