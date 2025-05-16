using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class OrderHistoryViewModel
    {
        public List<OrderSummaryViewModel> Orders { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
