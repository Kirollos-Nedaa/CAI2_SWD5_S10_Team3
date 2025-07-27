using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class OrderStatsChartViewModel
    {
        public List<string> Labels { get; set; } = new();
        public List<int> DataPoints { get; set; } = new();
        public int TotalOrders { get; set; }
        public double GrowthRate { get; set; }
    }
}
