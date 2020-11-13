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

using iRLeagueManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Enums;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.ViewModels
{
    public class UserViewModel : ContainerModelBase<UserModel>
    {
        internal UserModel Model => Source;
        public string UserId => (Model?.UserId);
        public string UserName
        {
            get => Model?.UserName;
            set
            {
                if (CheckUserName(value))
                {
                    Model.UserName = value;
                }
                else
                {
                    throw new InvalidOperationException("Invalid Username");
                }
            }
        }

        public long? MemberId { get => Model?.MemberId; set => Model.MemberId = value; }

        public AdminRights AdminRights => (Model?.AdminRights).GetValueOrDefault();

        public string Firstname 
        { 
            get => Model?.Firstname; 
            set
            {
                if (CheckName(value))
                {
                    Model.Firstname = value;
                }
                else
                {
                    throw new InvalidOperationException("Invalid Firstname");
                }
            }
        }

        public string Lastname 
        { 
            get => Model?.Lastname; 
            set
            {
                if (CheckName(value))
                {
                    Model.Lastname = value;
                }
                else
                {
                    throw new InvalidOperationException("Invalid Lastname");
                }
            }
        }

        public string Email { get => Model?.Email; set => Model.Email = value; }

        public string ProfileText { get => Model?.ProfileText; set => Model.ProfileText = value; }

        //public string FullName => Firstname + " " + Lastname;
        public string FullName
        {
            get
            {
                if (Model == null)
                    return "(null)";

                if (Firstname != null || Lastname != null)
                    return Firstname + " " + Lastname;

                if (UserName != null)
                    return UserName;

                return UserId;
            }
        }

        public UserViewModel()
        {

        }

        public async Task<bool> SaveChanges()
        {
            if (Model == null)
            {
                return false;
            }

            try
            {
                IsLoading = true;
                await LeagueContext.UserManager.PutUserModelAsync(Model);
                return true;
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool CheckUserName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                StatusMsg = "Please enter valid username";
                return false;
            }
            else if (name.Contains(' '))
            {
                StatusMsg = "Username cannot contain any spaces";
                return false;
            }
            return true;
        }

        private bool CheckName(string name)
        {
            return string.IsNullOrEmpty(name) == false;
        }
    }
}
