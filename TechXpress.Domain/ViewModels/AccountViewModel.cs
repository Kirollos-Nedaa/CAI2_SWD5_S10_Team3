using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class AccountViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
    }
}
