using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.domain.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(100, ErrorMessage = "Max length is 100")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [MaxLength(500, ErrorMessage = "Max length is 500")]
        public string Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime CommentDate { get; set; } = DateTime.Now;
        [ForeignKey("Post")]
        public int PostId { get; set; }
        [ValidateNever]
        public virtual Post Post { get; set; }
    }
}
