using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Cart
    {
        public int Cart_Id { get; set; }
        public int Customer_Id { get; set; }
        public DateTime Created_At { get; set; }

        // Navigation property
        public Customer Customer { get; set; }

        // Navigation properties for the related collections
        public ICollection<Cart_Items> Cart_Items { get; set; }
    }
}
