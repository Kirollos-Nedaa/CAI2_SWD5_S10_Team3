using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class GoogleLoginViewModel
    {
        [Required]
        public string IdToken { get; set; } = string.Empty;
    }
}
