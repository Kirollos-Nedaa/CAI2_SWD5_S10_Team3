using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class DashboardViewModel
    {
        public OrderStatsChartViewModel OrderStats { get; set; }
        public SalesStatsChartViewModel SalesStats { get; set; }
    }
}
