using System;
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
using System.Text.RegularExpressions;
using POS.DAL.Common;
using POS.WPF.Common;
using System.Windows;

namespace POS.WPF.Models.ViewModels
{
    public class UsersVM : BaseBindable
    {
        public UsersVM(IUserRepository userRepo, AppState appState)
        {
            _userRepo = userRepo;
            _appState = appState;

            LoadListCmd = new CommandAsync(LoadList);
            ShowFormCmd = new CommandAsyncParam(ShowForm);
            SuggestUsernameCmd = new CommandSync(SuggestUsername);
            SaveCmd = new CommandAsync(SaveForm);
            CancelCmd = new CommandSync(CancelForm);
            CheckAllCmd = new CommandParam(CheckAll);
            DeleteCmd = new CommandAsync(DeleteRows);
            RestoreCmd = new CommandAsync(RestoreRows);
        }

        private const string dialogHostId = "UsersDH";

        private readonly AppState _appState;
        private readonly IUserRepository _userRepo;

        public CommandAsync LoadListCmd { get; set; }
        public CommandAsyncParam ShowFormCmd { get; set; }
        public CommandSync SuggestUsernameCmd { get; set; }
        public CommandAsync SaveCmd { get; set; }
        public CommandSync CancelCmd { get; set; }
        public CommandParam CheckAllCmd { get; set; }
        public CommandAsync DeleteCmd { get; set; }
        public CommandAsync RestoreCmd { get; set; }

        public HeaderBarVM Header => new HeaderBarVM
        {
            HeaderText = "Users List",
            IconKind = PackIconKind.Add,
            ButtonCmd = new CommandAsyncParam(ShowForm)
        };

        private UserEM _currentUser = new UserEM();
        public UserEM CurrentUser
        {
            get { return _currentUser; }
            set { SetValue(ref _currentUser, value); }
        }

        private string _selectedUserRole;
        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set { SetValue(ref _selectedUserRole, value); }
        }

        private ObservableCollection<UserEM> _usersList;
        public ObservableCollection<UserEM> UsersList
        {
            get { return _usersList; }
            set { SetValue(ref _usersList, value); }
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

        private async Task LoadList()
        {
            await DialogHost.Show(new LoadingDialog(), dialogHostId, async (sender, args) =>
            {
                await GetList();
                args.Session.Close(false);
            },
            null);
        }

        private async Task GetList()
        {
            var result = await _userRepo.GetList(SelectedUserRole);
            var _data = result.Users.Select(u => new UserEM
            {
                Id = u.Id,
                DisplayName = u.DisplayName,
                Username = u.Username,
                UserRole = u.UserRole,
                IsDeleted = u.IsDeleted,
            });
            UsersList = new ObservableCollection<UserEM>(_data);
        }

        private async Task ShowForm(object id)
        {
            CurrentUser = new UserEM();
            await DialogHost.Show(new UserForm(), dialogHostId, async (sender, args)=>
            {
                dialogSession = args.Session;
                if (id != null)
                {
                    IsLoading = true;
                    var obj = await _userRepo.GetById((int)id);
                    CurrentUser = new UserEM
                    {
                        Id = obj.Id,
                        DisplayName = obj.DisplayName,
                        Username = obj.Username,
                        UserRole = obj.UserRole,
                    };
                    IsLoading = false;
                }
            },
            null);
        }

        private void SuggestUsername()
        {
            if (string.IsNullOrEmpty(CurrentUser.DisplayName) || CurrentUser.Id != 0) return;
            var username = CurrentUser.DisplayName.ToLower().Replace(" ", "_");
            username = Regex.Replace(username, @"[^a-zA-Z0-9_]", "");
            username = username.Substring(0, Math.Min(16, username.Length));
            CurrentUser.Username = username;
        }

        private void CancelForm()
        {
            CurrentUser = new UserEM();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async Task SaveForm()
        {
            CurrentUser.ValidateModel();
            if (CurrentUser.HasErrors) return;

            IsLoading = true;
            var data = new UserDTO
            {
                Id = CurrentUser.Id,
                DisplayName = CurrentUser.DisplayName,
                Username = CurrentUser.Username,
                Password = CurrentUser.Password,
                UserRole = CurrentUser.UserRole,
            };
            data.IsDeleted = false;
            data.UpdatedBy = _appState.CurrentUserId;
            data.UpdatedDate = DateTime.Now;

            try
            {

                if (CurrentUser.Id == 0)
                {
                    await _userRepo.Create(data);
                    CurrentUser = new UserEM();
                }
                else
                {
                    await _userRepo.Update(data);
                }
            }
            catch(MyException ex)
            {
                CurrentUser.AddManualError(nameof(CurrentUser.Username), ex.Message);
                IsLoading = false;
                return;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToMessage(), "System error occurred");
                IsLoading = false;
                return;
            }

            await GetList();
            IsLoading = false;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void CheckAll(object isChecked)
        {
            bool _isChecked = (bool)isChecked;
            foreach (var obj in UsersList) obj.IsChecked = _isChecked;
        }

        private async Task DeleteRows()
        {
            var ids = UsersList.Where(m => m.IsChecked).Select(m => m.Id).ToArray();
            if (ids.Length == 0) return;
            string message = $"Are you sure to delete ({ids.Length}) records?";
            var view = new ConfirmDialog(new MyDialogVM { Message = message });
            var obj = await DialogHost.Show(view, dialogHostId, (s, a) => dialogSession = a.Session, async (s, a) =>
            {
                if (a.Parameter is bool param && param == false) return;
                a.Cancel();
                a.Session.UpdateContent(new LoadingDialog());
                await _userRepo.Delete(ids);
                await GetList();
                a.Session.Close(false);
            });
        }

        private async Task RestoreRows()
        {
            var ids = UsersList.Where(m => m.IsChecked).Select(m => m.Id).ToArray();
            if (ids.Length == 0) return;

            var obj = await DialogHost.Show(new LoadingDialog(), dialogHostId, async (sender, args) =>
            {
                await _userRepo.Restore(ids);
                await GetList();
                args.Session.Close(false);
            }, null);
        }
    }
}
