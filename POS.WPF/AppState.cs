using POS.DAL.Models;
using POS.DAL.Query;
using POS.WPF.ViewModels;
using System.Threading.Tasks;

namespace POS.WPF
{
    public class AppState : BaseBindable
    {
        public AppState(UserQuery userQuery)
        {
            this.userQuery = userQuery;
        }

        private readonly UserQuery userQuery;

        private UserDTM _currentUser;
        public UserDTM CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public bool IsLoggedIn => CurrentUser != null;

        public async Task Login(string username, string password)
        {
            CurrentUser = await userQuery.Login(username, password);
        }

        public void Logout() => CurrentUser = null;
    }
}
