using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ViewModels
{
    public class ChangePasswordViewModel : UserViewModel
    {
        private string oldPassword;

        private string newPassword;

        private string confirmPassword;

        public void SetOldPassword(string pw)
        {
            oldPassword = pw;
        }

        public void SetNewPassword(string pw)
        {
            newPassword = pw;
        }

        public void SetConfirmPassword(string pw)
        {
            confirmPassword = pw;
        }

        public async Task<bool> SubmitAsync()
        {
            if (Model == null)
            {
                return false;
            }

            if (CheckPassword() == false)
            {
                return false;
            }

            try
            {
                IsLoading = true;
                var result = await LeagueContext.UserManager.ChangeUserPassword(Model.UserId, Model.UserName, oldPassword, newPassword);
                if (result == true)
                {
                    StatusMsg = "Password change succesful!";
                    return true;
                }
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
                StatusMsg = "Password change failed - " + e.Message;
            }
            finally
            {
                IsLoading = false;
            }

            return false;
        }

        public new bool CheckPassword()
        {
            if (oldPassword == null)
            {
                StatusMsg = "Old Password field empty. Please enter old password.";
            }
            else if (newPassword == null)
            {
                StatusMsg = "Password field empty. Please enter password.";
            }
            else if (newPassword.Length < 6)
            {
                StatusMsg = "Password must contain at least 6 characters.";
            }
            else if (confirmPassword == null || confirmPassword == "")
            {
                StatusMsg = "Confirm password field empty. Please confirm password";
            }
            else if (newPassword != confirmPassword)
            {
                StatusMsg = "Confirm password does not match.";
            }
            else
            {
                return true;
            }

            return false;
        }

        protected override void Dispose(bool disposing)
        {
            oldPassword = "";
            newPassword = "";
            confirmPassword = "";
            base.Dispose(disposing);
        }
    }
}
