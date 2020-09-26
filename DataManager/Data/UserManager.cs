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
    }
}
