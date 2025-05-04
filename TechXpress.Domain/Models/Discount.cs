using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
   public class Discount
    {
        public int Discount_Id { get; set; }
        public decimal Sale_amount { get; set; }
        public DateOnly Start_Date { get; set; }
        public DateOnly End_Date { get; set; }

        // Navigation properties for the related collections
        public ICollection<Discount_Item> Discount_Items { get; set; }
    }
}
