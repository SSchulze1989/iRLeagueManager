using iRLeagueManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Data
{
    public interface IUserManager : INotifyPropertyChanged
    {
        //string UserName { get; }
        UserModel CurrentUser { get; }
        bool IsAuthenticated { get; }
        Task<bool> UserLoginAsync(string userName, string password);
        void UserLogougt();
    }
}
