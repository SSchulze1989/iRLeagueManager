using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using iRLeagueManager.Models;
using iRLeagueManager.Data;
using iRLeagueManager.User;

namespace iRLeagueManager.ViewModels
{
    public class CreateUserViewModel : ViewModelBase
    {
        private MainWindowViewModel MainWindowVM { get; }

        //private LeagueContext LeagueContext => GlobalSettings.LeagueContext;
        private UserContext UserContext => GlobalSettings.UserContext;

        private UserModel user;
        public UserModel User { get => user; set => SetValue(ref user, value); }

        private string pw1;
        private string pw2;

        public string Pw1 { set => pw1 = value; }
        public string Pw2 { set => pw2 = value; }

        public ICommand SubmitButtonCommand { get; }
        public ICommand CancelButtonCommand { get; }

        private bool isOpen;
        public bool IsOpen { get => isOpen; set => SetValue(ref isOpen, value); }

        private bool isSubmitBusy = false;

        public CreateUserViewModel(MainWindowViewModel mainWindowVM)
        {
            MainWindowVM = mainWindowVM;
            User = new UserModel(0);
            SubmitButtonCommand = new RelayCommand(o => Submit(), o => CheckFields() && !isSubmitBusy);
            CancelButtonCommand = new RelayCommand(o => Cancel(), o => !isSubmitBusy);
        }

        private bool CheckFields()
        {
            bool result =
                User.UserName != "" && User.UserName != null &&
                User.Firstname != "" && User.Firstname != null &&
                User.Lastname != "" && User.Lastname != null &&
                pw1 != "" && pw1 != null &&
                pw2 != "" && pw2 != null &&
                pw1 == pw2;
            return result;
        }

        public void Open()
        {
            if (User == null)
                User = new UserModel(0);
            //MainWindowVM.PopUpVm = this;
            IsOpen = true;
        }

        public void Close()
        {
            //MainWindowVM.PopUpVm = MainWindowVM.UserLogin;
            IsOpen = false;
        }

        private async void Submit()
        {
            if (User != null && UserContext != null)
            {
                isSubmitBusy = true;
                await UserContext.CreateUserAsync(User, pw1);
                //newUser.Firstname = User.Firstname;
                //newUser.Lastname = User.Lastname;
                //newUser.Email = User.Email;
                //newUser.ProfileText = User.ProfileText;
                //await UserContext.PutUserDataAsync(newUser, pw1);
                isSubmitBusy = false;
                Close();
            }
        }

        private void Cancel()
        {
            User = null;
            Close();
        }
    }
}
