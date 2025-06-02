using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class WishlistViewModel
    {
        public List<WishlistItemViewModel> Items { get; set; } = new();
    }
}
