using AutoMapper;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Models.Mappings
{
    public class CurrencyRateProfile : Profile
    {
        public CurrencyRateProfile()
        {
            CreateMap<CurrencyRate, CurrencyRateDTO>()
                .ForMember(d => d.CurrencyName, opt => opt.MapFrom(s => s.Currency.Name))
                .ReverseMap()
                .ForMember(d => d.Currency, opt => opt.Ignore());

            CreateMap<CurrencyRateDTO, CurrencyRateEM>()
                .ReverseMap();
        }
    }
}
