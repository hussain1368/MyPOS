using AutoMapper;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.WPF.Enums;
using POS.WPF.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.WPF.Models.Mappings
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceDTO>()
                .ForMember(d => d.Items, opt => opt.MapFrom(s => s.InvoiceItems))
                .ReverseMap()
                .ForMember(d => d.ItemsCount, opt => opt.MapFrom(s => s.Items.Count()))
                .ForMember(d => d.TotalPrice, opt => opt.MapFrom(s => s.Items.Sum(i => i.TotalPrice)));

            CreateMap<InvoiceItem, InvoiceItemDTO>()
                .ForMember(d => d.ProductCode, opt => opt.MapFrom(s => s.Product.Code))
                .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.Name))
                .ForMember(d => d.CurrencyId, opt => opt.MapFrom(s => s.Invoice.CurrencyId))
                .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(s => s.Invoice.Currency.Code))
                .ReverseMap()
                .ForMember(d => d.Product, opt => opt.Ignore());

            CreateMap<Invoice, InvoiceRowDTO>()
                .ForMember(d => d.WalletName, opt => opt.MapFrom(s => s.Wallet.Name))
                .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(s => s.Currency.Code))
                .ForMember(d => d.CurrencyName, opt => opt.MapFrom(s => s.Currency.Name))
                .ForMember(d => d.PartnerName, opt => opt.MapFrom(s => s.Partner != null ? s.Partner.Name : null));

            CreateMap<InvoiceDTO, InvoiceEM>()
                .ForMember(d => d.PaymentType, opt => opt.MapFrom(s => (PaymentType)s.PaymentType))
                .ReverseMap()
                .ForMember(d => d.PaymentType, opt => opt.MapFrom(s => (byte)s.PaymentType))
                //.ForMember(d => d.CurrencyId, opt => opt.MapFrom(s => s.Wallet.CurrencyId))
                .ForMember(d => d.WalletId, opt => opt.MapFrom(s => s.Wallet.Id));

            CreateMap<InvoiceItemDTO, InvoiceItemEM>()
                .ReverseMap();
        }
    }
}
