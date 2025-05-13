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
        public string UserId { get; set; }  // Changed to string for ASP.NET Identity
        public DateTime Created_At { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ApplicationUser User { get; set; }
        public ICollection<Cart_Items> CartItems { get; set; } = new List<Cart_Items>();
    }
}
