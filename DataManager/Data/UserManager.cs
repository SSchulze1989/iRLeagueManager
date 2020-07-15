using iRLeagueManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Data
{
    public class UserManager : NotifyPropertyChangedBase, IUserManager
    {
        IUserCredentialsManager CredentialsManager { get; }

        //public string UserName => CredentialsManager?.AuthenticatedUserName;

        public bool IsAuthenticated => CredentialsManager.IsAuthenticated;

        private UserModel currentUser;
        public UserModel CurrentUser { get => currentUser; set { currentUser = value; OnPropertyChanged(); } }

        public UserManager(IUserCredentialsManager credentialsManager)
        {
            CredentialsManager = credentialsManager;
        }

        public async Task<bool> UserLoginAsync(string userName, string password)
        {
            var result = await CredentialsManager.AuthenticateAsync(userName, password);
            if (result == true)
            {
                CurrentUser = new UserModel(0)
                {
                    UserName = userName
                };
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
    }
}
