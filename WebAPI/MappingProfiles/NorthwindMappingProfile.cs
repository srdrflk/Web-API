using AutoMapper;
using WebAPI.Models.DTOs;
using WebAPI.Models;

namespace WebAPI.MappingProfiles
{
    public class NorthwindMappingProfile : Profile
    {
        public NorthwindMappingProfile()
        {
            // Category mappings
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<CreateCategoryDTO, Category>();

            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty));

            // Product mappings
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null));

            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();

        }
    }
}
