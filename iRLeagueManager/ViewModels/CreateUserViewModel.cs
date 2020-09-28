// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Data;
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

        public async Task<bool> SubmitAsync()
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
                if (e is UserExistsException)
                {
                    StatusMsg = "Failed to Register\nUsername \"" + UserName + "\" is already in use.";
                    IsLoading = false;
                    return false;
                }

                GlobalSettings.LogError(e);

                StatusMsg = "Failed to Register\n" + e.Message;
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
            if (password == null)
            {
                StatusMsg = "Password field empty. Please enter password.";
            }
            else if (password.Length < 6)
            {
                StatusMsg = "Password must contain at least 6 characters.";
            }
            else if (confirmPassword == null || confirmPassword == "")
            {
                StatusMsg = "Confirm password field empty. Please confirm password";
            }
            else if (password != confirmPassword)
            {
                StatusMsg = "Confirm password does not match.";
            }
            else
            {
                return true;
            }

            return false;
        }

        public bool CanSubmit()
        {
            StatusMsg = "";

            if (UserName == null || UserName == "")
            {
                StatusMsg = "Username field empty. Please enter a valid username.";
                return false;
            }
            else if (UserName.Contains(' '))
            {
                StatusMsg = "Username invalid. Username can not contain spaces.";
                return false;
            }
            else if (Firstname == null || Firstname == "")
            {
                StatusMsg = "Firstname field empty. Please enter a valid name.";
                return false;
            }
            else if (Lastname == null || Lastname == "")
            {
                StatusMsg = "Lastname field empty. Please enter a valid name.";
                return false;
            }
            else if (CheckPassword() == false)
            {
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
