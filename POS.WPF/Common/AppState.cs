using POS.DAL.DTO;
using POS.DAL.Repository;
using POS.WPF.Models;
using System.Threading.Tasks;

namespace POS.WPF.Common
{
    public class AppState : BaseBindable
    {
        public AppState(UserRepository userQuery)
        {
            this.userQuery = userQuery;
        }

        private readonly UserRepository userQuery;

        private UserDTO _currentUser = new UserDTO { DisplayName = "Hussain Hussaini" };
        public UserDTO CurrentUser
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
