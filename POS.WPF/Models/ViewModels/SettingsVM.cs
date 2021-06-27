using POS.DAL.DTO;
using POS.WPF.Commands;
using POS.WPF.Common;
using POS.WPF.Models.EntityModels;
using System;
using System.Threading.Tasks;

namespace POS.WPF.Models.ViewModels
{
    public class SettingsVM : BaseBindable
    {
        public SettingsVM(AppState appState)
        {
            LoadSettingCmd = new CommandSync(LoadSetting);
            SaveCmd = new CommandAsync(Save);
            this.appState = appState;
        }

        private readonly AppState appState;

        public CommandAsync SaveCmd { get; set; }
        public CommandSync LoadSettingCmd { get; set; }

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
                AppTitle = appState.Settings.AppTitle,
                Language = appState.Settings.Language,
                CalendarType = (Enums.CalendarType)appState.Settings.CalendarType,
            };
        }

        private async Task Save()
        {
            var data = new SettingDTO
            {
                Id = appState.Settings.Id,
                AppTitle = CurrentSetting.AppTitle,
                Language = CurrentSetting.Language,
                CalendarType = (byte)CurrentSetting.CalendarType,
            };
            await appState.UpdateSettings(data);
        }
    }
}
