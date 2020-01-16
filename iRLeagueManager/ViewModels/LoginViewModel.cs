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
        private MainWindowViewModel MainWindowVM { get; }

        private UserContext UserContext => GlobalSettings.UserContext;

        //private UserModel loginUser;
        //public UserModel LoginUser { get => loginUser; set => SetValue(ref loginUser, value); }

        private string userName;
        public string UserName { get => userName; set => SetValue(ref userName, value); }

        private string pw;
        public string Pw { get => pw; set => SetValue(ref pw, value); }

        private bool isOpen;
        public bool IsOpen { get => isOpen; set => SetValue(ref isOpen, value); }

        private string statusMessage;
        public string StatusMessage { get => statusMessage; set => SetValue(ref statusMessage, value); }

        public ICommand SubmitButtonCommand { get; }
        public ICommand CloseButtonCommand { get; }
        public ICommand RegisterUserCommand { get; }

        public LoginViewModel(MainWindowViewModel mainVM)
        {
            MainWindowVM = mainVM;
            UserName = "";
            Pw = "";

            SubmitButtonCommand = new RelayCommand(o => Submit(), o => UserName != "" && Pw != "");
            CloseButtonCommand = new RelayCommand(o => Close(), o => true);
            RegisterUserCommand = new RelayCommand(o => RegisterOpen(), o => true);
        }

        private async void Submit()
        {
            if (await Login())
            {
                IsOpen = false;
                //MainWindowVM.Connect();
            }
        }

        private void RegisterOpen()
        {
            var createUserVM = new CreateUserViewModel(MainWindowVM);
            createUserVM.Open();
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

            if (Pw == "")
            {
                //throw new UserValidationExeption("Passowrd is empty. Please enter a valid Password");
                StatusMessage = "Passowrd is empty. Please enter a valid Password";
                return false;
            }

            var result = await UserContext.UserLoginAsync(UserName, Pw);
            if (!result)
            {
                StatusMessage = "Password or Username incorrect!";
            }
            return result;
        }

        public void SetPassword(string pw)
        {
            Pw = pw;
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
