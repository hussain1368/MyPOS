using AutoMapper;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Localization;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using POS.WPF.Commands;
using POS.WPF.Common;
using POS.WPF.Models.EntityModels;
using POS.WPF.Views.Sections;
using POS.WPF.Views.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace POS.WPF.Models.ViewModels
{
    public class SettingsVM : BaseBindable
    {
        public SettingsVM(AppState appState,
            IOptionRepository optionRepo,
            ICurrencyRateRepository currencyRateRepo,
            IStringLocalizer<Labels> _t,
            IMapper mapper)
        {
            _mapper = mapper;
            _appState = appState;
            _optionRepo = optionRepo;
            _currencyRateRepo = currencyRateRepo;

            TabChangedCmd = new CommandAsyncParam(TabChanged);
            LoadSettingCmd = new CommandSync(LoadSetting);
            LoadOptionsCmd = new CommandAsync(LoadOptions);
            SaveSettingCmd = new CommandAsync(SaveSetting);

            OpenFormCmd = new CommandAsyncParam(OpenForm);
            SaveOptionCmd = new CommandAsync(SaveOption);
            CancelOptionCmd = new CommandSync(CancelOption);
            CheckAllCmd = new CommandParam(CheckAll);
            DeleteOptionsCmd = new CommandAsync(DeleteOptions);

            DeleteCurrencyRateCmd = new CommandAsync(DeleteCurrencyRate);
            CheckAllCurrencyRateCmd = new CommandParam(CheckAllCurrencyRate);
            OpenCurrencyFormCmd = new CommandAsyncParam(OpenCurrencyForm);
            LoadCurrencyRatesCmd = new CommandAsyncParam(LoadCurrencyRates);
            SaveCurrencyRateCmd = new CommandAsync(SaveCurrencyRate);
            CancelCurrencyRateCmd = new CommandSync(CancelCurrencyRate);

            HeaderContext = new HeaderBarVM
            {
                HeaderText = _t["Settings"],
                IsButtonVisible = false,
            };
        }

        private readonly IMapper _mapper;
        private readonly AppState _appState;
        private readonly IOptionRepository _optionRepo;
        private readonly ICurrencyRateRepository _currencyRateRepo;

        public CommandAsync SaveSettingCmd { get; set; }
        public CommandAsync LoadOptionsCmd { get; set; }
        public CommandSync LoadSettingCmd { get; set; }
        public CommandAsyncParam OpenFormCmd { get; set; }
        public CommandAsync SaveOptionCmd { get; set; }
        public CommandSync CancelOptionCmd { get; set; }
        public CommandParam CheckAllCmd { get; set; }
        public CommandAsync DeleteOptionsCmd { get; set; }
        public CommandAsyncParam TabChangedCmd { get; set; }

        public CommandAsync DeleteCurrencyRateCmd { get; set; }
        public CommandParam CheckAllCurrencyRateCmd { get; set; }
        public CommandAsyncParam OpenCurrencyFormCmd { get; set; }
        public CommandAsyncParam LoadCurrencyRatesCmd { get; set; }
        public CommandAsync SaveCurrencyRateCmd { get; set; }
        public CommandSync CancelCurrencyRateCmd { get; set; }

        private HeaderBarVM _headerContext;
        public HeaderBarVM HeaderContext
        {
            get => _headerContext;
            set => SetValue(ref _headerContext, value);
        }

        private SettingEM _currentSetting = new SettingEM();
        public SettingEM CurrentSetting
        {
            get { return _currentSetting; }
            set { SetValue(ref _currentSetting, value); }
        }

        private IEnumerable<OptionTypeDTO> _optionTypes = Enumerable.Empty<OptionTypeDTO>();
        public IEnumerable<OptionTypeDTO> OptionTypes
        {
            get { return _optionTypes; }
            set
            {
                SetValue(ref _optionTypes, value);
                OnPropertyChanged(nameof(FormOptionTypes));
            }
        }

        public IEnumerable<OptionTypeDTO> FormOptionTypes => OptionTypes.Where(t => !t.IsReadOnly).ToList();

        private IEnumerable<OptionValueDTO> _optionValues = Enumerable.Empty<OptionValueDTO>();
        public IEnumerable<OptionValueDTO> OptionValues
        {
            get { return _optionValues; }
            set
            {
                SetValue(ref _optionValues, value);
                OnPropertyChanged(nameof(SelectedTypeOptions));
                OnPropertyChanged(nameof(CurrencyList));
                OnPropertyChanged(nameof(CanAddOption));
            }
        }

        private OptionTypeDTO _selectedType;
        public OptionTypeDTO SelectedType
        {
            get { return _selectedType; }
            set
            {
                SetValue(ref _selectedType, value);
                OnPropertyChanged(nameof(CanAddOption));

                SelectedTypeOptions = OptionValues.Where(v => v.TypeId == SelectedType?.Id)
                    .Select(op => new OptionValueEM
                    {
                        Id = op.Id,
                        Code = op.Code,
                        TypeId = op.TypeId,
                        Name = op.Name,
                        IsDefault = op.IsDefault,
                        IsReadOnly = op.IsReadOnly,
                        IsDeleted = op.IsDeleted,
                    })
                    .ToList();
            }
        }

        public IEnumerable<OptionValueEM> _selectedTypeOptions = Enumerable.Empty<OptionValueEM>();
        public IEnumerable<OptionValueEM> SelectedTypeOptions
        {
            get { return _selectedTypeOptions; }
            set { SetValue(ref _selectedTypeOptions, value); }
        }


        public bool CanAddOption => !SelectedType?.IsReadOnly ?? false;

        private OptionValueEM _currentOption = new OptionValueEM();
        public OptionValueEM CurrentOption
        {
            get { return _currentOption; }
            set { SetValue(ref _currentOption, value); }
        }

        private bool _isOptionLoading;
        public bool IsOptionLoading
        {
            get { return _isOptionLoading; }
            set { SetValue(ref _isOptionLoading, value); }
        }

        private void LoadSetting()
        {
            CurrentSetting = new SettingEM
            {
                Id = _appState.Settings.Id,
                AppTitle = _appState.Settings.AppTitle,
                Language = _appState.Settings.Language,
                CalendarType = (Enums.CalendarType)_appState.Settings.CalendarType,
            };
        }

        private async Task LoadOptions()
        {
            await DialogHost.Show(new LoadingDialog(), "SettingsDH", async (sender, args) =>
            {
                OptionTypes = await _optionRepo.OptionTypes();
                await GetOptions();
                args.Session.Close();
            },
            null);
        }

        private async Task GetOptions()
        {
            OptionValues = await _optionRepo.OptionsAll(true);
            // work here ...

        }

        private async Task SaveSetting()
        {
            await DialogHost.Show(new LoadingDialog(), "SettingsDH", async (sender, args) =>
            {
                var data = new SettingDTO
                {
                    Id = CurrentSetting.Id,
                    AppTitle = CurrentSetting.AppTitle,
                    Language = CurrentSetting.Language,
                    CalendarType = (byte)CurrentSetting.CalendarType,
                };
                await _appState.UpdateSettings(data);
                args.Session.Close();
            },
            null);
        }

        private void CancelOption()
        {
            CurrentOption = new OptionValueEM();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async Task SaveOption()
        {
            CurrentOption.ValidateModel();
            if (CurrentOption.HasErrors) return;

            var data = new OptionValueDTO
            {
                Id = CurrentOption.Id,
                Code = CurrentOption.Code,
                TypeId = CurrentOption.TypeId.Value,
                Name = CurrentOption.Name,
            };
            IsOptionLoading = true;
            if (CurrentOption.Id == 0)
                await _optionRepo.CreateOption(data);
            else
                await _optionRepo.UpdateOption(data);
            await GetOptions();
            IsOptionLoading = false;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async Task OpenForm(object id)
        {
            if (id != null)
            {
                var option = OptionValues.FirstOrDefault(op => op.Id == (int)id);
                CurrentOption = new OptionValueEM
                {
                    Id = option.Id,
                    TypeId = option.TypeId,
                    Code = option.Code,
                    Name = option.Name,
                };
            }
            else
            {
                CurrentOption = new OptionValueEM
                {
                    TypeId = SelectedType?.Id ?? 0
                };
            }
            await DialogHost.Show(new OptionForm(), "SettingsDH");
        }

        private void CheckAll(object isChecked)
        {
            bool _isChecked = (bool)isChecked;
            foreach (var obj in SelectedTypeOptions)
            {
                if (!obj.IsReadOnly) obj.IsChecked = _isChecked;
            }
        }

        private async Task DeleteOptions()
        {
            var ids = SelectedTypeOptions.Where(m => m.IsChecked && !m.IsReadOnly).Select(m => m.Id).ToArray();
            if (ids.Length == 0) return;
            string message = $"Are you sure to delete ({ids.Length}) options?";
            var view = new ConfirmDialog(new MyDialogVM { Message = message });
            var obj = await DialogHost.Show(view, "SettingsDH", null, async (sender, args) =>
            {
                if (args.Parameter is bool param && param == false) return;
                args.Cancel();
                args.Session.UpdateContent(new LoadingDialog());
                await _optionRepo.DeleteOptions(ids);
                await GetOptions();
                args.Session.Close(false);
            });
        }

        private IEnumerable<CurrencyRateEM> _currencyRates = Enumerable.Empty<CurrencyRateEM>();
        public IEnumerable<CurrencyRateEM> CurrencyRates
        {
            get { return _currencyRates; }
            set
            {
                SetValue(ref _currencyRates, value);
            }
        }

        private async Task TabChanged(object e)
        {
            var args = (SelectionChangedEventArgs)e;
            var name = ((FrameworkElement)args.OriginalSource).Name;
            if (name != "Tabs") return;

            if (SelectedTab == 2) await LoadCurrencyRates(true);
        }

        private async Task LoadCurrencyRates(object showLoading)
        {
            var LoadThem = async () =>
            {
                var data = await _currencyRateRepo.GetList(SelectedCurrencyId);
                CurrencyRates = _mapper.Map<IEnumerable<CurrencyRateEM>>(data);
            };

            if ((bool)showLoading == false)
            {
                await LoadThem();
            }
            else
            {
                await DialogHost.Show(new LoadingDialog(), "SettingsDH", async (sender, args) =>
                {
                    await LoadThem();
                    args.Session.Close();
                },
                null);
            }
        }

        public IList<OptionValueDTO> CurrencyList => OptionValues
            .Where(v => v.TypeCode == "CRC" && v.IsDeleted == false && v.IsDefault == false)
            .ToList();

        private OptionValueDTO _selectedCurrency;
        public OptionValueDTO SelectedCurrency
        {
            get { return _selectedCurrency; }
            set
            {
                SetValue(ref _selectedCurrency, value);
            }
        }

        private int? _selectedCurrencyId;
        public int? SelectedCurrencyId
        {
            get { return _selectedCurrencyId; }
            set { SetValue(ref _selectedCurrencyId, value); }
        }

        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set { SetValue(ref _selectedTab, value); }
        }

        private CurrencyRateEM _currentCurrencyRate = new CurrencyRateEM();
        public CurrencyRateEM CurrentCurrencyRate
        {
            get { return _currentCurrencyRate; }
            set { SetValue(ref _currentCurrencyRate, value); }
        }

        private bool _isCurrencyRateLoading;
        public bool IsCurrencyRateLoading
        {
            get { return _isCurrencyRateLoading; }
            set
            {
                SetValue(ref _isCurrencyRateLoading, value);
                OnPropertyChanged(nameof(CurrencyRateButtonsEnabled));
            }
        }

        private async Task OpenCurrencyForm(object id)
        {
            CurrentCurrencyRate = new CurrencyRateEM
            {
                CurrencyId = SelectedCurrencyId,
                RateDate = DateTime.Now,
            };
            await DialogHost.Show(new CurrencyRateForm(), "SettingsDH");
        }

        private void CancelCurrencyRate()
        {
            CurrentCurrencyRate = new CurrencyRateEM();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async Task SaveCurrencyRate()
        {
            CurrentCurrencyRate.ValidateModel();
            if (CurrentCurrencyRate.HasErrors) return;

            IsCurrencyRateLoading = true;
            var data = _mapper.Map<CurrencyRateDTO>(CurrentCurrencyRate);
            data.IsDeleted = false;
            data.UpdatedBy = 1;
            data.UpdatedDate = DateTime.Now;

            await _currencyRateRepo.Create(data);
            await LoadCurrencyRates(false);

            IsCurrencyRateLoading = false;
            CurrentCurrencyRate = new CurrencyRateEM();

            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void CheckAllCurrencyRate(object isChecked)
        {
            bool _isChecked = (bool)isChecked;
            foreach (var obj in CurrencyRates) obj.IsChecked = _isChecked;
        }

        private async Task DeleteCurrencyRate()
        {
            var ids = CurrencyRates.Where(m => m.IsChecked).Select(m => m.Id).ToArray();
            if (ids.Length == 0) return;
            string message = $"Are you sure to delete ({ids.Length}) records?";
            var view = new ConfirmDialog(new MyDialogVM { Message = message });
            var obj = await DialogHost.Show(view, "SettingsDH", null, async (sender, args) =>
            {
                if (args.Parameter is bool param && param == false) return;
                args.Cancel();
                args.Session.UpdateContent(new LoadingDialog());
                await _currencyRateRepo.Delete(ids);
                await LoadCurrencyRates(false);
                args.Session.Close(false);
            });
        }

        public bool CurrencyRateButtonsEnabled => !IsCurrencyRateLoading;
    }
}
