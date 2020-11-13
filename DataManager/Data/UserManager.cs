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
using iRLeagueManager.Models.User;
using iRLeagueDatabase.DataTransfer.User;

namespace iRLeagueManager.Data
{
    public class UserManager : NotifyPropertyChangedBase, IUserManager
    {
        IUserCredentialsManager CredentialsManager { get; }

        IUserDatabaseClient UserDatabaseClient { get; }

        IModelCache ModelCache { get; }

        //public string UserName => CredentialsManager?.AuthenticatedUserName;

        public bool IsAuthenticated => CredentialsManager.IsAuthenticated;

        private UserModel currentUser;
        public UserModel CurrentUser { get => currentUser; set { currentUser = value; OnPropertyChanged(); } }

        public UserManager(IModelCache modelCache, IUserCredentialsManager credentialsManager, IUserDatabaseClient client)
        {
            CredentialsManager = credentialsManager;
            UserDatabaseClient = client;
            ModelCache = modelCache;
        }

        public async Task<bool> UserLoginAsync(string userName, string password)
        {
            var result = await CredentialsManager.AuthenticateAsync(userName, password);
            if (result == true)
            {
                var user = await GetUserModelAsync(userName);
                CurrentUser = user;
            }
            else
            {
                CurrentUser = UserModel.GetAnonymous();
            }
            return result;
        }

        public void UserLogougt()
        {
            CredentialsManager.ClearCredentials();
            CurrentUser = UserModel.GetAnonymous();
        }

        public UserModel GetUserModel(string userId)
        {
            if (userId == null)
                return null;

            UserModel userModel = ModelCache.GetModel<UserModel>(userId);

            if (userModel != null)
                return userModel;

            userModel = new UserModel(userId);

            ModelCache.PutModel(userModel);

            _ = Task.Run(async () => 
            {
                var getModel = await GetUserModelAsync(userId, true);
                if (getModel != null)
                {
                    userModel.CopyFrom(getModel);
                }
            });

            return userModel;
        }

        public async Task<UserModel> GetUserModelAsync(string userIdOrName)
            => await GetUserModelAsync(userIdOrName, forceUpdate: false);

        public async Task<UserModel> GetUserModelAsync(string userIdOrName, bool forceUpdate)
        {
            UserModel userModel = ModelCache.GetModel<UserModel>(userIdOrName);

            if (userModel != null && forceUpdate == false)
                return userModel;

            var userDto = await UserDatabaseClient.GetUserAsync(userIdOrName);

            if (userDto != null)
            {
                if (userModel == null)
                {
                    userModel = new UserModel(userDto.UserId);
                    userModel = ModelCache.PutOrGetModel(userModel);
                }
                userModel.UserName = userDto.UserName;
                userModel.Firstname = userDto.Firstname;
                userModel.MemberId = userDto.MemberId;
                userModel.Lastname = userDto.Lastname;

                return userModel;
            }
            return null;
        }

        public async Task<UserModel> AddUserModelAsync(UserModel user, string password)
        {
            if (user == null)
                return null;
            if (password == null || password == "")
                throw new ArgumentException("Failed to add user. Password can not be empty");

            var userDto = new AddUserDTO()
            {
                UserName = user.UserName,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Password = password
            };

            var userProfileDto = await UserDatabaseClient.AddUserAsync(userDto);

            if (userProfileDto != null)
            {
                user.UserId = userProfileDto.UserId;
                return user;
            }
            else
            {
                throw new Exception("Failed to Add User");
            }
        }

        public async Task<UserModel> PutUserModelAsync(UserModel user)
        {
            if (user == null)
            {
                return null;
            }

            var userDto = new UserProfileDTO()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                ProfileText = user.ProfileText
            };

            userDto = await UserDatabaseClient.PutUserAsync(userDto);

            if (userDto != null)
            {
                user.UserId = userDto.UserId;
                user.Firstname = userDto.Firstname;
                user.UserName = userDto.UserName;
                user.Lastname = userDto.Lastname;
                user.Email = userDto.Email;
                user.ProfileText = userDto.ProfileText;
            }

            return user;
        }

        public async Task<bool> ChangeUserPassword(string userId, string userName, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("New passwort may not be null or empty");
            }

            try
            {
                var userDto = new AddUserDTO()
                {
                    UserId = userId,
                    UserName = userName,
                    Password = newPassword
                };
                var result = await UserDatabaseClient.ChangePassword(userName, oldPassword, userDto);
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
