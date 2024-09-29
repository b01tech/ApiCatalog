using ApiCatalog.Models;
using AutoMapper;

namespace ApiCatalog.DTOs.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<CategoryDto, CategoryDTO>().ReverseMap();

        }
    }
}
