using AutoMapper;
using technical_tests_backend_ssr.Models.DTOs;
using technical_tests_backend_ssr.Models.Entities;

namespace technical_tests_backend_ssr.MappingServices
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
