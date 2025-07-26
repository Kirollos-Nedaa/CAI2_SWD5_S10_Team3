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
        public string UserId { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
