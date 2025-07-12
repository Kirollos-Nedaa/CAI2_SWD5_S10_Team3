using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? VerificationCode { get; set; }
    }

}
