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
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [MinLength(2,ErrorMessage ="Min length is 2"),MaxLength(100, ErrorMessage = "max length is 100")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [MinLength(10, ErrorMessage = "Min length is 10")]
        public string Content { get; set; }
        [ValidateNever]
        public string Image { get; set; } // note ? IFormFile
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public virtual Category Category { get; set; } // object
        [ValidateNever]
        public virtual ICollection<Comment> Comments { get; set; } // collection

    }
}
