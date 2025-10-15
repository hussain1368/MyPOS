using AutoMapper;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Models.Mappings
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
             CreateMap<Transaction, TransactionDTO>()
                .ForMember(d => d.CurrencyName, opt => opt.MapFrom(s => s.Currency.Name))
                .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(s => s.Currency.Code));

            CreateMap<TransactionDTO, TransactionEM>();
        }
    }
}
