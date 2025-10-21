using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using POS.WPF.Models;
using System.Globalization;
using System.Threading.Tasks;

namespace POS.WPF.Common
{
    public class AppState : BaseBindable
    {
        public AppState(IUserRepository userRepo, IOptionRepository optionsRepo)
        {
            this.userRepo = userRepo;
            this.optionsRepo = optionsRepo;
        }

        private readonly IUserRepository userRepo;
        private readonly IOptionRepository optionsRepo;

        private UserDTO _currentUser = new UserDTO { DisplayName = "Hussain Hussaini", UserRole="Simple" };
        public UserDTO CurrentUser
        {
            get { return _currentUser; }
            set
            {
                SetValue(ref _currentUser, value);
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        private SettingDTO _settings;
        public SettingDTO Settings
        {
            get { return _settings; }
            set
            {
                SetValue(ref _settings, value);
                OnPropertyChanged(nameof(LayoutDirection));
            }
        }

        public string LayoutDirection => Settings.Language == "en-US" ? "LeftToRight" : "RightToLeft";

        public bool IsLoggedIn => CurrentUser != null;

        public async Task Login(string username, string password)
        {
            CurrentUser = await userRepo.Login(username, password);
        }

        public void Logout() => CurrentUser = null;

        public void LoadSettings()
        {
            Settings = optionsRepo.GetActiveSetting();
            CultureInfo.CurrentUICulture = new CultureInfo(Settings.Language, false);
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("prs-AF");
        }

        public async Task UpdateSettings(SettingDTO data)
        {
            await optionsRepo.UpdateSetting(data);
            Settings = data;
        }
    }
}
