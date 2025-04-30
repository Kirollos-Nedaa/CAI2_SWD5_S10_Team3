using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.domain.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        public string Email { set; get; }
        [Required]
        public string Password { set; get; }

    }
}
