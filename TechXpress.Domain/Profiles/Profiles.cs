using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;

namespace TechXpress.Domain.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name));

            CreateMap<CreateProductDto, Product>();


            CreateMap<Product, EditProductDto>();
            CreateMap<EditProductDto, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Condition(src => src.ImageFile == null)); // Preserve existing ImageUrl if no new file 


            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.Img, opt => opt.Ignore()); // We set this manually


            CreateMap<Category, EditCategoryDto>();


            CreateMap<EditCategoryDto, Category>()
                .ForMember(dest => dest.Img, opt => opt.Condition(src => src.ImageFile == null)); // Preserve existing ImageUrl if no new file

            CreateMap<CreateBrandDto, Brand>()
                .ForMember(dest => dest.imgUrl, opt => opt.Ignore());

            CreateMap<Brand, EditBrandDto>();


            CreateMap<EditBrandDto, Brand>()
                .ForMember(dest => dest.imgUrl, opt => opt.Ignore());
        }
    }
    
}
