using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
   public class Discount_Sales
    {
        public int Id { get; set; }
        public DateOnly Start_Date { get; set; }

        public DateOnly End_Date { get; set; }

        public decimal Sale_amount { get; set; }

    }
}
