using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class VerifyCodeViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "The code must be a 6-digit number.")]
        public string Code { get; set; }
    }
}
