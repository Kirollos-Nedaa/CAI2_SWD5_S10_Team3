using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class AddressDto
    {
        public int Address_Id { get; set; }
        public string FullName { get; set; }
        public string AddressType { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public string Buillding { get; set; }
        public int Floor { get; set; }
        public int Flat { get; set; }
        public string PostCode { get; set; }
        public bool IsDefault { get; set; }
    }
}
