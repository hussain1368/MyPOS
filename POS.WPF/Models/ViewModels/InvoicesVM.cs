using AutoMapper;
using MaterialDesignThemes.Wpf;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using POS.WPF.Commands;
using POS.WPF.Enums;
using POS.WPF.Views.Sections;
using POS.WPF.Views.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.WPF.Models.ViewModels
{
    public class InvoicesVM : BaseBindable
    {
        public InvoicesVM(InvoiceFormVM invoiceFormContext, IInvoiceRepository invoiceRepo, IMapper mapper)
        {
            _mapper = mapper;
            _invoiceRepo = invoiceRepo;

            LoadListCmd = new CommandAsync(LoadList);
            ShowFormCmd = new CommandAsyncParam(ShowForm);
            InvoiceFormContext = invoiceFormContext;
            invoiceFormContext.ParentPage = this;

            HeaderContext = new HeaderBarVM
            {
                HeaderText = "Sale and Purchase",
                IconKind = PackIconKind.ArrowBack,
                IsButtonVisible = false,
                ButtonCmd = new CommandSync(() =>
                {
                    HeaderContext.IsButtonVisible = false;
                    HeaderContext.HeaderText = "Sale and Purchase";
                    TransitionerIndex--;
                })
            };
        }

        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IMapper _mapper;

        public CommandAsync LoadListCmd { get; set; }
        public CommandAsyncParam ShowFormCmd { get; set; }
        public InvoiceFormVM InvoiceFormContext { get; }

        private HeaderBarVM _headerContext;
        public HeaderBarVM HeaderContext
        {
            get => _headerContext;
            set => SetValue(ref _headerContext, value);
        }

        private int _transitionerIndex = 0;
        public int TransitionerIndex
        {
            get => _transitionerIndex;
            set => SetValue(ref _transitionerIndex, value);
        }

        private IEnumerable<InvoiceRowDTO> _invoicesList = Enumerable.Empty<InvoiceRowDTO>();
        public IEnumerable<InvoiceRowDTO> InvoicesList
        {
            get => _invoicesList;
            set => SetValue(ref _invoicesList, value);
        }

        private InvoiceType? _invoiceType;
        public InvoiceType? InvoiceType
        {
            get => _invoiceType;
            set => SetValue(ref _invoiceType, value);
        }

        private DateTime? _issueDate;
        public DateTime? IssueDate
        {
            get => _issueDate;
            set => SetValue(ref _issueDate, value);
        }

        public async Task LoadList()
        {
            await DialogHost.Show(new LoadingDialog(), "MainDialogHost", async (sender, args) =>
            {
                var data = await _invoiceRepo.GetList((byte?)InvoiceType, IssueDate, PageIndex);
                InvoicesList = data.Invoices;
                PageCount = data.PageCount;

                args.Session.Close(false);
            },
            null);
        }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set => SetValue(ref _pageIndex, value);
        }

        private int _pageCount = 0;
        public int PageCount
        {
            get => _pageCount;
            set => SetValue(ref _pageCount, value);
        }

        private async Task ShowForm(object param)
        {
            TransitionerIndex++;
            HeaderContext.IsButtonVisible = true;
            if (param is string)
            {
                var invoiceType = Enum.Parse<InvoiceType>(param.ToString());
                InvoiceFormContext.ClearForm(invoiceType);
            }
            if (param is int _param)
            {
                await InvoiceFormContext.SetInvoiceData(_param);
            }
            switch (InvoiceFormContext.InvoiceType)
            {
                case Enums.InvoiceType.Sale: HeaderContext.HeaderText = "Sale Invoice Details"; break;
                case Enums.InvoiceType.Purchase: HeaderContext.HeaderText = "Purchase Invoice Details"; break;
                case Enums.InvoiceType.Return: HeaderContext.HeaderText = "Return Invoice Details"; break;
            }
        }
    }
}
