using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.ViewModels
{
    public class CreateUserViewModel : UserViewModel
    {
        public new string UserName { get => Model.UserName; set => Model.UserName = value; }

        private string password;
        private string confirmPassword;

        string passwordStatus;
        public string PasswordStatus { get => passwordStatus; set => SetValue(ref passwordStatus, value); }

        public CreateUserViewModel() 
        {
            SetSource(new UserModel(null));
        }

        public void SetPassword(string password)
        {
            this.password = password;
        }

        public void SetConfirmPassword(string password)
        {
            confirmPassword = password;
        }

        public async Task<bool> Submit()
        {
            if (Model == null)
                return false;

            UserModel user = null;

            try
            {
                IsLoading = true;
                user = await LeagueContext.UserManager.AddUserModelAsync(Model, password);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }

            if (user != null)
            {
                SetSource(user);
                return true;
            }
            return false;
        }

        public bool CheckPassword()
        {
            if (password == null || confirmPassword == null)
            {
                PasswordStatus = "";
                return false;
            }

            if (password != confirmPassword)
            {
                PasswordStatus = "Confirm password does not match.";
                return false;
            }

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            password = null;
            confirmPassword = null;
            base.Dispose(disposing);
        }
    }
}
