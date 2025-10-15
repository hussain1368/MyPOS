using AutoMapper;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Models.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductItemDTO>();

            CreateMap<Product, ProductDTO>()
               .ForMember(d => d.UnitName, opt => opt.MapFrom(s => s.Unit != null ? s.Unit.Name : null))
               .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.Brand != null ? s.Brand.Name : null))
               .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.Name : null))
               .ForMember(d => d.CurrencyName, opt => opt.MapFrom(s => s.Currency.Name))
               .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(s => s.Currency.Code))
               .ReverseMap()
               .ForMember(s => s.Unit, opt => opt.Ignore())
               .ForMember(s => s.Brand, opt => opt.Ignore())
               .ForMember(s => s.Category, opt => opt.Ignore())
               .ForMember(s => s.Currency, opt => opt.Ignore());

            CreateMap<ProductDTO, ProductEM>().ReverseMap();
        }
    }
}
