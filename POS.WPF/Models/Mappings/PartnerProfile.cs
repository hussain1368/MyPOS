using AutoMapper;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Models.Mappings
{
    public class PartnerProfile : Profile
    {
        public PartnerProfile()
        {
            CreateMap<Partner, PartnerDTO>()
                .ReverseMap()
                .ForMember(d => d.PartnerType, opt => opt.Ignore())
                .ForMember(d => d.Currency, opt => opt.Ignore());

            CreateMap<PartnerDTO, PartnerEM>()
                .ReverseMap();
        }
    }
}
