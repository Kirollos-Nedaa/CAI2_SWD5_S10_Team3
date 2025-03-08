using System;
using System.Collections.Generic;

namespace TechXpress.Domain.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string PhNo { get; set; }
        public string Password { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public ICollection<Wishlist> Wishlists { get; set; }

        public ICollection<Address> Addresses { get; set; }

    }
}