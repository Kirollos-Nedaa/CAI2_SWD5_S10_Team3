using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category image is required")]
        public IFormFile ImageFile { get; set; }
    }
}
