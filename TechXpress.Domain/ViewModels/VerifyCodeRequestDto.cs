using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class VerifyCodeRequest
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}
