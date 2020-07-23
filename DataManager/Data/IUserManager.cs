using iRLeagueManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.Data
{
    public interface IUserManager : INotifyPropertyChanged
    {
        //string UserName { get; }
        UserModel CurrentUser { get; }
        bool IsAuthenticated { get; }
        Task<bool> UserLoginAsync(string userName, string password);
        UserModel GetUserModel(string userId);
        Task<UserModel> GetUserModelAsync(string userId);
        Task<UserModel> AddUserModelAsync(UserModel user, string password);
        void UserLogougt();
    }
}
