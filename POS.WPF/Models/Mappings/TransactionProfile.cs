using AutoMapper;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.WPF.Enums;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Models.Mappings
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDTO>()
               .ForMember(d => d.TreasuryName, opt => opt.MapFrom(s => s.Treasury.Name))
               .ForMember(d => d.SourceName, opt => opt.MapFrom(s => s.Source.Name))
               .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(s => s.Currency.Code))
               .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(s => s.Currency.Code))
               .ReverseMap()
               .ForMember(d => d.Treasury, opt => opt.Ignore())
               .ForMember(d => d.Source, opt => opt.Ignore())
               .ForMember(d => d.Currency, opt => opt.Ignore());

            CreateMap<TransactionDTO, TransactionEM>()
                .ForMember(d => d.TransactionType, opt => opt.MapFrom(s => (TransactionType)s.TransactionType))
                .ReverseMap()
                .ForMember(d => d.TransactionType, opt => opt.MapFrom(s => (byte)s.TransactionType));
        }
    }
}
