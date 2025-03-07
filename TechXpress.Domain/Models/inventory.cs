using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class inventory
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int Amount_on_hand { get; set; }

        public Product Product { get; set; }
    }
}
