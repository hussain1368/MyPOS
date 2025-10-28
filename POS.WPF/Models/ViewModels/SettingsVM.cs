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
            SaveSettingCmd = new CommandAsync(SaveSetting);

            LoadOptionsCmd = new CommandAsync(LoadOptions);
            OpenOptionFormCmd = new CommandAsyncParam(OpenOptionForm);
            SaveOptionCmd = new CommandAsync(SaveOption);
            CancelOptionCmd = new CommandSync(CancelOption);
            CheckAllOptionsCmd = new CommandParam(CheckAllOptions);
            DeleteOptionsCmd = new CommandAsync(DeleteOptions);
            RestoreOptionsCmd = new CommandAsync(RestoreOptions);

            LoadCurrencyRatesCmd = new CommandAsync(LoadCurrencyRates);
            OpenCurrencyFormCmd = new CommandAsyncParam(OpenCurrencyForm);
            SaveCurrencyRateCmd = new CommandAsync(SaveCurrencyRate);
            CancelCurrencyRateCmd = new CommandSync(CancelCurrencyRate);
            CheckAllCurrencyRateCmd = new CommandParam(CheckAllCurrencyRate);
            DeleteCurrencyRateCmd = new CommandAsync(DeleteCurrencyRate);

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
        private const string dialogHost = "SettingsDH"; 

        public CommandAsyncParam TabChangedCmd { get; set; }
        public CommandSync LoadSettingCmd { get; set; }
        public CommandAsync SaveSettingCmd { get; set; }

        public CommandAsync LoadOptionsCmd { get; set; }
        public CommandAsyncParam OpenOptionFormCmd { get; set; }
        public CommandAsync SaveOptionCmd { get; set; }
        public CommandSync CancelOptionCmd { get; set; }
        public CommandParam CheckAllOptionsCmd { get; set; }
        public CommandAsync DeleteOptionsCmd { get; set; }
        public CommandAsync RestoreOptionsCmd { get; set; }

        public CommandAsync LoadCurrencyRatesCmd { get; set; }
        public CommandAsyncParam OpenCurrencyFormCmd { get; set; }
        public CommandAsync SaveCurrencyRateCmd { get; set; }
        public CommandSync CancelCurrencyRateCmd { get; set; }
        public CommandParam CheckAllCurrencyRateCmd { get; set; }
        public CommandAsync DeleteCurrencyRateCmd { get; set; }

        private HeaderBarVM _headerContext;
        public HeaderBarVM HeaderContext
        {
            get => _headerContext;
            set => SetValue(ref _headerContext, value);
        }

        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set { SetValue(ref _selectedTab, value); }
        }

        private async Task TabChanged(object e)
        {
            var args = (SelectionChangedEventArgs)e;
            var name = ((FrameworkElement)args.OriginalSource).Name;
            if (name != "Tabs") return;

            if (SelectedTab == 1) await LoadOptions();
        }

        private SettingEM _currentSetting = new SettingEM();
        public SettingEM CurrentSetting
        {
            get { return _currentSetting; }
            set { SetValue(ref _currentSetting, value); }
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

        private async Task SaveSetting()
        {
            await DialogHost.Show(new LoadingDialog(), dialogHost, async (sender, args) =>
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

        // ============== Options Tab ==============

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

                SetSelectedOptionValues();
            }
        }

        private void SetSelectedOptionValues()
        {
            SelectedTypeOptions = OptionValues
                .Where(v => v.TypeId == SelectedType?.Id)
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

        private async Task LoadOptions()
        {
            await DialogHost.Show(new LoadingDialog(), dialogHost, async (sender, args) =>
            {
                OptionTypes = await _optionRepo.OptionTypes();
                await GetOptionsValues();
                args.Session.Close();
            },
            null);
        }

        private async Task GetOptionsValues()
        {
            OptionValues = await _optionRepo.OptionsAll(true);
            SetSelectedOptionValues();
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

            await GetOptionsValues();

            IsOptionLoading = false;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async Task OpenOptionForm(object id)
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
            await DialogHost.Show(new OptionForm(), dialogHost);
        }

        private void CheckAllOptions(object isChecked)
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
            var obj = await DialogHost.Show(view, dialogHost, null, async (sender, args) =>
            {
                if (args.Parameter is bool param && param == false) return;
                args.Cancel();
                args.Session.UpdateContent(new LoadingDialog());

                await _optionRepo.DeleteOptions(ids);
                await GetOptionsValues();

                args.Session.Close(false);
            });
        }

        private async Task RestoreOptions()
        {
            var ids = SelectedTypeOptions.Where(m => m.IsChecked && !m.IsReadOnly).Select(m => m.Id).ToArray();
            if (ids.Length == 0) return;

            await DialogHost.Show(new LoadingDialog(), dialogHost, async (sender, args) =>
            {
                await _optionRepo.RestoreOptions(ids);
                await GetOptionsValues();
                args.Session.Close(false);
            }, null);
        }

        // ============== Currency Rate Tab ==============

        private IEnumerable<CurrencyRateEM> _currencyRates = Enumerable.Empty<CurrencyRateEM>();
        public IEnumerable<CurrencyRateEM> CurrencyRates
        {
            get { return _currencyRates; }
            set
            {
                SetValue(ref _currencyRates, value);
            }
        }

        private async Task LoadCurrencyRates()
        {
            await DialogHost.Show(new LoadingDialog(), dialogHost, async (sender, args) =>
            {
                if (!CurrencyList.Any())
                {
                    var currencies = await _optionRepo.OptionsByTypeCode("CRC");
                    CurrencyList = currencies.Select(c => new OptionValueDTO
                    {
                        Id = c.Id,
                        Code = c.Code,
                        Name = c.Name,
                    });
                }

                await GetCurrencyRates();
                args.Session.Close();
            },
            null);
        }

        private async Task GetCurrencyRates()
        {
            var data = await _currencyRateRepo.GetList(SelectedCurrencyId, SelectedRateDate);
            CurrencyRates = _mapper.Map<IEnumerable<CurrencyRateEM>>(data);
        }

        private IEnumerable<OptionValueDTO> _currencyList = Enumerable.Empty<OptionValueDTO>();
        public IEnumerable<OptionValueDTO> CurrencyList
        {
            get => _currencyList;
            set => SetValue(ref _currencyList, value);
        }

        private int? _selectedCurrencyId;
        public int? SelectedCurrencyId
        {
            get { return _selectedCurrencyId; }
            set { SetValue(ref _selectedCurrencyId, value); }
        }

        private DateTime? _selectedRateDate = DateTime.Now;
        public DateTime? SelectedRateDate
        {
            get { return _selectedRateDate; }
            set { SetValue(ref _selectedRateDate, value); }
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
            await DialogHost.Show(new CurrencyRateForm(), dialogHost);
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
            data.UpdatedBy = _appState.CurrentUserId;
            data.UpdatedDate = DateTime.Now;

            await _currencyRateRepo.Create(data);
            await GetCurrencyRates();

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
            var obj = await DialogHost.Show(view, dialogHost, null, async (sender, args) =>
            {
                if (args.Parameter is bool param && param == false) return;
                args.Cancel();
                args.Session.UpdateContent(new LoadingDialog());
                await _currencyRateRepo.Delete(ids);
                await GetCurrencyRates();
                args.Session.Close(false);
            });
        }

        public bool CurrencyRateButtonsEnabled => !IsCurrencyRateLoading;
    }
}
