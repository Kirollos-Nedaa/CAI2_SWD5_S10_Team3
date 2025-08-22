using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public string? ReplacedByToken { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ApplicationUser User { get; set; }
    }
}
