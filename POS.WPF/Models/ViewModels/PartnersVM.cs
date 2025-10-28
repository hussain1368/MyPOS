using AutoMapper;
using MaterialDesignThemes.Wpf;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using POS.WPF.Commands;
using POS.WPF.Common;
using POS.WPF.Models.EntityModels;
using POS.WPF.Views.Sections;
using POS.WPF.Views.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace POS.WPF.Models.ViewModels
{
    public class PartnersVM : BaseBindable
    {
        public PartnersVM(IPartnerRepository partnerRepo, IOptionRepository optionRepo, IMapper mapper, AppState appState)
        {
            _partnerRepo = partnerRepo;
            _optionRepo = optionRepo;
            _mapper = mapper;
            _appState = appState;

            LoadOptionsCmd = new CommandAsync(LoadOptions);
            LoadListCmd = new CommandAsync(LoadList);
            ShowFormCmd = new CommandAsyncParam(ShowForm);
            SaveCmd = new CommandAsync(SaveForm);
            CancelCmd = new CommandSync(CancelForm);
            CheckAllCmd = new CommandParam(CheckAll);
            DeleteCmd = new CommandAsync(DeleteRows);
        }

        private readonly AppState _appState;
        private readonly IPartnerRepository _partnerRepo;
        private readonly IOptionRepository _optionRepo;
        private readonly IMapper _mapper;

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
            IconKind = PackIconKind.Add,
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

        private async Task LoadOptions()
        {
            ComboOptions = await _optionRepo.OptionsAll();
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
            var result = await _partnerRepo.GetList(PartnerTypeId, PageIndex);
            var _data = _mapper.Map<IEnumerable<PartnerEM>>(result.Partners);
            PartnersList = new ObservableCollection<PartnerEM>(_data);
            PageCount = result.PageCount;
        }

        private async Task ShowForm(object id)
        {
            CurrentPartner = new PartnerEM();
            await DialogHost.Show(new PartnerForm(), "PartnersDH", async (sender, args)=>
            {
                dialogSession = args.Session;
                if (id != null)
                {
                    IsLoading = true;
                    var obj = await _partnerRepo.GetById((int)id);
                    CurrentPartner = _mapper.Map<PartnerEM>(obj);
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
            var data = _mapper.Map<PartnerDTO>(CurrentPartner);
            data.IsDeleted = false;
            data.UpdatedBy = _appState.CurrentUserId;
            data.UpdatedDate = DateTime.Now;

            if (CurrentPartner.Id == 0)
            {
                await _partnerRepo.Create(data);
                CurrentPartner = new PartnerEM();
            }
            else
            {
                await _partnerRepo.Update(data);
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
            var obj = await DialogHost.Show(view, "PartnersDH", (s, a) => dialogSession = a.Session, async (s, a) =>
            {
                if (a.Parameter is bool param && param == false) return;
                a.Cancel();
                a.Session.UpdateContent(new LoadingDialog());
                await _partnerRepo.Delete(ids);
                await GetList();
                a.Session.Close(false);
            });
        }
    }
}
