using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace TechXpress.Domain.Models
{
    public class Customer
    {
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string PhNo { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime Created_At { get; set; }

        // Navigation properties for the related collections
        public ICollection<Wishlist> Wishlists { get; set; }
        public ICollection<Address> Addresses { get; set; }

    }
}