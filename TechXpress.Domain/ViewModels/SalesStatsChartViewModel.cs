using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class SalesStatsChartViewModel
    {
        public List<string> Labels { get; set; } = new();
        public List<int> DataPoints { get; set; } = new();
        public decimal TotalSales { get; set; }
        public decimal GrowthRate { get; set; }
        public string RangeLabel { get; set; }
    }
}
