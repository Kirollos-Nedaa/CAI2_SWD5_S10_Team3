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
        public int Oreder_Id { get; set; }
        public string Stripe_payment_intent_id { get; set; }
        public decimal Amount { get; set; }
        public string Payment_type { get; set; }
        public DateTime Payment_date { get; set; }
        public string Status { get; set; }

        // Navigation property
        public Orders Orders { get; set; }
    }
}
