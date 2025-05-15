using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Address
    {
        public int Address_Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Apartment { get; set; }
        public string PostCode { get; set; }
        public bool IsDefault { get; set; }

        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }
    }
}