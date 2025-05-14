using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Domain.ViewModels
{
    public class EditBrandDto
    {
        public int Brand_Id { get; set; }

        [Required(ErrorMessage = "Brand name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        public string ImgUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
