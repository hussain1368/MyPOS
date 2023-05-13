using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Localization;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using POS.WPF.Commands;
using POS.WPF.Common;
using POS.WPF.Models.EntityModels;
using POS.WPF.Views.Sections;
using POS.WPF.Views.Shared;
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
            IStringLocalizer<Labels> _t)
        {
            this.appState = appState;
            this.optionRepo = optionRepo;
            this.currencyRateRepo = currencyRateRepo;
            LoadSettingCmd = new CommandSync(LoadSetting);
            LoadOptionsCmd = new CommandAsync(LoadOptions);
            SaveSettingCmd = new CommandAsync(SaveSetting);
            OpenFormCmd = new CommandAsyncParam(OpenForm);
            SaveOptionCmd = new CommandAsync(SaveOption);
            CancelOptionCmd = new CommandSync(CancelOption);
            CheckAllCmd = new CommandParam(CheckAll);
            DeleteOptionsCmd = new CommandAsync(DeleteOptions);
            LoadCurrencyRatesCmd = new CommandAsync(LoadCurrencyRates);
            TabChangedCmd = new CommandAsyncParam(TabChanged);

            HeaderContext = new HeaderBarVM
            {
                HeaderText = _t["Settings"],
                IsButtonVisible = false,
            };
        }

        private readonly AppState appState;
        private readonly IOptionRepository optionRepo;
        private readonly ICurrencyRateRepository currencyRateRepo;

        public CommandAsync SaveSettingCmd { get; set; }
        public CommandAsync LoadOptionsCmd { get; set; }
        public CommandSync LoadSettingCmd { get; set; }
        public CommandAsyncParam OpenFormCmd { get; set; }
        public CommandAsync SaveOptionCmd { get; set; }
        public CommandSync CancelOptionCmd { get; set; }
        public CommandParam CheckAllCmd { get; set; }
        public CommandAsync DeleteOptionsCmd { get; set; }
        public CommandAsync LoadCurrencyRatesCmd { get; set; }
        public CommandAsyncParam TabChangedCmd { get; set; }

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
                Id = appState.Settings.Id,
                AppTitle = appState.Settings.AppTitle,
                Language = appState.Settings.Language,
                CalendarType = (Enums.CalendarType)appState.Settings.CalendarType,
            };
        }

        private async Task LoadOptions()
        {
            await DialogHost.Show(new LoadingDialog(), "SettingsDH", async (sender, args) =>
            {
                OptionTypes = await optionRepo.OptionTypes();
                await GetOptions();
                args.Session.Close();
            },
            null);
        }

        private async Task GetOptions()
        {
            OptionValues = await optionRepo.OptionsAll(true);
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
                await appState.UpdateSettings(data);
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
                await optionRepo.CreateOption(data);
            else
                await optionRepo.UpdateOption(data);
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
            var view = new ConfirmDialog(new ConfirmDialogVM { Message = message });
            var obj = await DialogHost.Show(view, "SettingsDH", null, async (sender, args) =>
            {
                if (args.Parameter is bool param && param == false) return;
                args.Cancel();
                args.Session.UpdateContent(new LoadingDialog());
                await optionRepo.DeleteOptions(ids);
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

            if (SelectedTab == 2) await LoadCurrencyRates();
        }

        private async Task LoadCurrencyRates()
        {
            await DialogHost.Show(new LoadingDialog(), "SettingsDH", async (sender, args) =>
            {
                var data = await currencyRateRepo.GetList(SelectedCurrencyId);
                CurrencyRates = data.Select(r => new CurrencyRateEM
                {
                    Id = r.Id,
                    CurrencyId = r.CurrencyId,
                    CurrencyName = r.CurrencyName,
                    RateDate = r.RateDate,
                    BaseValue = r.BaseValue,
                    Rate = r.Rate,
                    ReverseCalculation = r.ReverseCalculation,
                    FinalRate = r.FinalRate,
                    Note = r.Note,
                    IsDeleted = r.IsDeleted,
                })
                .ToList();
                args.Session.Close();
            },
            null);
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
    }
}
