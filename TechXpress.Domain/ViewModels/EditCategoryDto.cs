using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class EditCategoryDto
    {
        public int Category_Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        public string Img { get; set; }  // Current image path
        public IFormFile? ImageFile { get; set; }  // New uploaded file
    }
}
