using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using iRLeagueManager.Models;
using iRLeagueManager.User;
using iRLeagueManager.Interfaces;
using System.Runtime.CompilerServices;

namespace iRLeagueManager.ViewModels
{
    public class LoginViewModel : ViewModelBase, IHasPassword
    {
        //private MainWindowViewModel MainWindowVM { get; }

        //private UserModel loginUser;
        //public UserModel LoginUser { get => loginUser; set => SetValue(ref loginUser, value); }

        private string userName;
        public string UserName { get => userName; set => SetValue(ref userName, value); }

        private string password;

        private bool isOpen;
        public bool IsOpen { get => isOpen; set => SetValue(ref isOpen, value); }

        private bool isLoggedIn;
        public bool IsLoggedIn { get => isLoggedIn; set => SetValue(ref isLoggedIn, value); }

        private string statusMessage;
        public string StatusMessage { get => statusMessage; set => SetValue(ref statusMessage, value); }

        public ICommand SubmitButtonCommand { get; }
        public ICommand CloseButtonCommand { get; }
        public ICommand RegisterUserCommand { get; }

        public LoginViewModel()
        {
            UserName = "";
            password = "";

            SubmitButtonCommand = new RelayCommand(async o => await SubmitAsync(), o => UserName != "" && password != "" && !IsLoading);
            CloseButtonCommand = new RelayCommand(o => Close(), o => true);
            RegisterUserCommand = new RelayCommand(o => RegisterOpen(), o => true);
        }

        public void Load()
        {
            if (GlobalSettings.LeagueContext == null)
                GlobalSettings.SetGlobalLeagueContext(new Data.LeagueContext());
        }

        public async Task SubmitAsync()
        {
            try
            {
                IsLoading = true;
                if (await Login())
                {
                    IsOpen = false;
                    IsLoggedIn = true;
                    //MainWindowVM.Connect();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                IsLoading = false;
            }
            //MainWindowVM.Refresh("CurrentUser");
            //MainWindowVM.Load();
        }

        private void RegisterOpen()
        {
            //var createUserVM = new CreateUserViewModel(MainWindowVM);
            //createUserVM.Open();
        }

        public void Open()
        {
            //MainWindowVM.PopUpVm = this;
            IsOpen = true;
        }

        public void Close()
        {
            IsOpen = false;
            //MainWindowVM.PopUpVm = null;
            StatusMessage = "";
        }

        private async Task<bool> Login()
        {
            if (UserName == "")
            {
                //throw new UserValidationExeption("Username is empty. Please enter a valid Username");
                StatusMessage = "Username is empty. Please enter a valid Username";
                return false;
            }

            if (password == "")
            {
                //throw new UserValidationExeption("Passowrd is empty. Please enter a valid Password");
                StatusMessage = "Passowrd is empty. Please enter a valid Password";
                return false;
            }

            var result = await LeagueContext.UserLoginAsync(UserName, password);
            if (result == false)
            {
                StatusMessage = "Password or Username incorrect!";
            }
            return result;
        }

        public void SetPassword(string password)
        {
            this.password = password;
        }
    }


    [Serializable]
    public class UserValidationExeption : Exception
    {
        public UserValidationExeption() { }
        public UserValidationExeption(string message) : base(message) { }
        public UserValidationExeption(string message, Exception inner) : base(message, inner) { }
        protected UserValidationExeption(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
