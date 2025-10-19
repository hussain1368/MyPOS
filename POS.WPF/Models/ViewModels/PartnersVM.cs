using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using POS.DAL.DTO;
using POS.WPF.Commands;
using POS.WPF.Views.Sections;
using POS.WPF.Models.EntityModels;
using POS.WPF.Views.Shared;
using POS.DAL.Repository.Abstraction;

namespace POS.WPF.Models.ViewModels
{
    public class PartnersVM : BaseBindable
    {
        public PartnersVM(IPartnerRepository partnerRepo, IOptionRepository optionRepo)
        {
            this.partnerRepo = partnerRepo;
            this.optionRepo = optionRepo;

            LoadOptionsCmd = new CommandAsync(LoadOptions);
            LoadListCmd = new CommandAsync(LoadList);
            ShowFormCmd = new CommandAsyncParam(ShowForm);
            SaveCmd = new CommandAsync(SaveForm);
            CancelCmd = new CommandSync(CancelForm);
            CheckAllCmd = new CommandParam(CheckAll);
            DeleteCmd = new CommandAsync(DeleteRows);
        }

        private readonly IPartnerRepository partnerRepo;
        private readonly IOptionRepository optionRepo;

        public CommandAsync LoadOptionsCmd { get; set; }
        public CommandAsync LoadListCmd { get; set; }
        public CommandAsyncParam ShowFormCmd { get; set; }
        public CommandAsync SaveCmd { get; set; }
        public CommandSync CancelCmd { get; set; }
        public CommandParam CheckAllCmd { get; set; }
        public CommandAsync DeleteCmd { get; set; }

        public HeaderBarVM Header => new HeaderBarVM
        {
            HeaderText = "Partners List",
            IconKind = "Add",
            ButtonCmd = new CommandAsyncParam(ShowForm)
        };

        private IList<OptionValueDTO> _comboOptions;
        public IList<OptionValueDTO> ComboOptions
        {
            get { return _comboOptions; }
            set
            {
                _comboOptions = value;
                OnPropertyChanged(nameof(CurrencyList));
                OnPropertyChanged(nameof(PartnerTypeList));
            }
        }

        public IList<OptionValueDTO> CurrencyList => ComboOptions?.Where(op => op.TypeCode == "CRC").ToList();
        public IList<OptionValueDTO> PartnerTypeList => ComboOptions?.Where(op => op.TypeCode == "ATYP").ToList();

        private PartnerEM _currentPartner = new PartnerEM();
        public PartnerEM CurrentPartner
        {
            get { return _currentPartner; }
            set { SetValue(ref _currentPartner, value); }
        }

        private ObservableCollection<PartnerEM> _partnersList;
        public ObservableCollection<PartnerEM> PartnersList
        {
            get { return _partnersList; }
            set { SetValue(ref _partnersList, value); }
        }

        private int? _partnerTypeId;
        public int? PartnerTypeId
        {
            get { return _partnerTypeId; }
            set { SetValue(ref _partnerTypeId, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetValue(ref _isLoading, value); }
        }

        private async Task LoadOptions()
        {
            ComboOptions = await optionRepo.OptionsAll();
        }

        private async Task LoadList()
        {
            await DialogHost.Show(new LoadingDialog(), "PartnersDH", async (sender, args) =>
            {
                await GetList();
                args.Session.Close(false);
            },
            null);
        }

        private async Task GetList()
        {
            var data = await partnerRepo.GetList(PartnerTypeId);
            var _data = data.Select(m => new PartnerEM
            {
                Id = m.Id,
                Name = m.Name,
                Phone = m.Phone,
                Address = m.Address,
                Note = m.Note,
                CurrencyId = m.CurrencyId,
                CurrentBalance = m.CurrentBalance,
                PartnerTypeId = m.PartnerTypeId,
                PartnerTypeName = m.PartnerTypeName,
                CurrencyName = m.CurrencyName,
                CurrencyCode = m.CurrencyCode,
            });
            PartnersList = new ObservableCollection<PartnerEM>(_data);
        }

        private async Task ShowForm(object id)
        {
            CurrentPartner = new PartnerEM();
            await DialogHost.Show(new PartnerForm(), "PartnersDH", async (sender, args)=>
            {
                if (id != null)
                {
                    IsLoading = true;
                    var obj = await partnerRepo.GetById((int)id);
                    CurrentPartner = new PartnerEM
                    {
                        Id = obj.Id,
                        Name = obj.Name,
                        Phone = obj.Phone,
                        Address = obj.Address,
                        Note = obj.Note,
                        CurrencyId = obj.CurrencyId,
                        CurrentBalance = obj.CurrentBalance,
                        PartnerTypeId = obj.PartnerTypeId,
                    };
                    IsLoading = false;
                }
            },
            null);
        }

        private void CancelForm()
        {
            CurrentPartner = new PartnerEM();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async Task SaveForm()
        {
            CurrentPartner.ValidateModel();
            if (CurrentPartner.HasErrors) return;

            IsLoading = true;
            var data = new PartnerDTO
            {
                Id = CurrentPartner.Id,
                Name = CurrentPartner.Name,
                Phone = CurrentPartner.Phone,
                Address = CurrentPartner.Address,
                Note = CurrentPartner.Note,
                CurrencyId = CurrentPartner.CurrencyId.Value,
                PartnerTypeId = CurrentPartner.PartnerTypeId.Value,
                CurrentBalance = CurrentPartner.CurrentBalance,
                IsDeleted = false,
                UpdatedBy = 1,
                UpdatedDate = DateTime.Now,
            };

            if (CurrentPartner.Id == 0)
            {
                await partnerRepo.Create(data);
                CurrentPartner = new PartnerEM();
            }
            else
            {
                await partnerRepo.Update(data);
            }
            await GetList();
            IsLoading = false;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void CheckAll(object isChecked)
        {
            bool _isChecked = (bool)isChecked;
            foreach (var obj in PartnersList) obj.IsChecked = _isChecked;
        }

        private async Task DeleteRows()
        {
            var ids = PartnersList.Where(m => m.IsChecked).Select(m => m.Id).ToArray();
            if (ids.Length == 0) return;
            string message = $"Are you sure to delete ({ids.Length}) records?";
            var view = new ConfirmDialog(new MyDialogVM { Message = message });
            var obj = await DialogHost.Show(view, "PartnersDH", null, async (sender, args) =>
            {
                if (args.Parameter is bool param && param == false) return;
                args.Cancel();
                args.Session.UpdateContent(new LoadingDialog());
                await partnerRepo.Delete(ids);
                await GetList();
                args.Session.Close(false);
            });
        }
    }
}
