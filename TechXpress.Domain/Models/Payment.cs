using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class Payment
    {
        public int Payment_Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        
    }
}
