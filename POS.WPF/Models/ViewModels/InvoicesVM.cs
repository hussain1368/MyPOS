using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using POS.DAL.DTO;
using POS.DAL.Query;
using POS.WPF.Commands;
using POS.WPF.Enums;
using POS.WPF.Views.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.WPF.Models.ViewModels
{
    public class InvoicesVM : BaseBindable
    {
        public InvoicesVM(InvoiceFormVM invoiceFormContext, InvoiceQuery invoiceQuery)
        {
            LoadListCmd = new RelayCommandAsync(LoadList);
            ShowFormCmd = new RelayCommandAsyncParam(ShowForm);
            InvoiceFormContext = invoiceFormContext;
            invoiceFormContext.ParentPage = this;
            this.invoiceQuery = invoiceQuery;
        }

        private readonly InvoiceQuery invoiceQuery;

        public RelayCommandAsync LoadListCmd { get; set; }
        public RelayCommandAsyncParam ShowFormCmd { get; set; }
        public InvoiceFormVM InvoiceFormContext { get; }

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
                InvoicesList = await invoiceQuery.GetList((byte?)InvoiceType, IssueDate);
                args.Session.Close(false);
            },
            null);
        }

        private async Task ShowForm(object param)
        {
            Transitioner.MoveNextCommand.Execute(null, null);
            if (param is string)
            {
                var invoiceType = Enum.Parse<InvoiceType>(param.ToString());
                InvoiceFormContext.ClearForm(invoiceType);
            }
            if (param is int _param)
            {
                await InvoiceFormContext.SetInvoiceData(_param);
            }
        }
    }
}
