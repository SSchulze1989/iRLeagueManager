using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.UserDBServiceRef;
using iRLeagueManager.Models.Database;
using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.Data
{
    public class DbUserServiceClient : DbServiceClientBase, IUserService
    {
        private string EndpointConfigurationName { get; } = "";

        private UserServiceClient DbClient
        {
            get
            {
                if (ConnectionStatus == Enums.ConnectionStatusEnum.Disconnected)
                    //Status.ConnectionStatus = Enums.ConnectionStatusEnum.Conecting;
                    SetConnectionStatus(Token, ConnectionStatusEnum.Connecting);

                var retVal = (EndpointConfigurationName == "") ? new UserServiceClient() : new UserServiceClient(EndpointConfigurationName);

                if (ConnectionStatus == Enums.ConnectionStatusEnum.Connecting)
                    //Status.ConnectionStatus = Enums.ConnectionStatusEnum.Connected;
                    SetConnectionStatus(Token, ConnectionStatusEnum.Connected);

                return retVal;
            }
        }

        public DbUserServiceClient(IDatabaseStatus status) : base(status)
        {
        }

        public DbUserServiceClient(IDatabaseStatus status, string endpointConfigurationName) : this(status)
        {
            EndpointConfigurationName = endpointConfigurationName;
        }

        public UserDTO UserLogin(string userName, string pw)
        {
            return ((IUserService)DbClient).UserLogin(userName, pw);
        }

        public async Task<UserDTO> UserLoginAsync(string userName, string pw)
        {
            IToken token = new RequestToken();
            await StartUpdateWhenReady(UpdateKind.Loading, token);
            var retVal = await ((IUserService)DbClient).UserLoginAsync(userName, pw);
            EndUpdate(token);
            return retVal;
        }

        public bool UserSetPw(int userId, string oldPw, string newPw)
        {
            return ((IUserService)DbClient).UserSetPw(userId, oldPw, newPw);
        }

        public async Task<bool> UserSetPwAsync(int userId, string oldPw, string newPw)
        {
            IToken token = new RequestToken();
            await StartUpdateWhenReady(UpdateKind.Saving, token);
            var retVal = await ((IUserService)DbClient).UserSetPwAsync(userId, oldPw, newPw);
            EndUpdate(token);
            return retVal;
        }

        public bool IsUserNameAvailable(string username)
        {
            return ((IUserService)DbClient).IsUserNameAvailable(username);
        }

        public async Task<bool> IsUserNameAvailableAsync(string username)
        {
            IToken token = new RequestToken();
            await StartUpdateWhenReady(UpdateKind.Loading, token);
            var retVal = await ((IUserService)DbClient).IsUserNameAvailableAsync(username);
            EndUpdate(token);
            return retVal;
        }

        public UserDTO CreateUser(string userName, string initialPw)
        {
            return ((IUserService)DbClient).CreateUser(userName, initialPw);
        }

        public async Task<UserDTO> CreateUserAsync(string userName, string initialPw)
        {
            IToken token = new RequestToken();
            await StartUpdateWhenReady(UpdateKind.Saving, token);
            var retVal = await ((IUserService)DbClient).CreateUserAsync(userName, initialPw);
            EndUpdate(token);
            return retVal;
        }

        //public UserDTO SetAdminRights(UserDTO user, AdminRights rights)
        //{
        //    return ((IUserService)DbClient).SetAdminRights(user, rights);
        //}

        //public async Task<UserDTO> SetAdminRightsAsync(UserDTO user, AdminRights rights)
        //{
        //    await StartUpdateWhenReady(UpdateKind.Saving);
        //    var retVal = await ((IUserService)DbClient).SetAdminRightsAsync(user, rights);
        //    EndUpdate();
        //    return retVal;
        //}

        public UserDTO GetUserData(int userId)
        {
            return ((IUserService)DbClient).GetUserData(userId);
        }

        public async Task<UserDTO> GetUserDataAsync(int userId)
        {
            IToken token = new RequestToken();
            await StartUpdateWhenReady(UpdateKind.Loading, token);
            var retVal = await ((IUserService)DbClient).GetUserDataAsync(userId);
            EndUpdate(token);
            return retVal;
        }

        public UserDTO[] GetUserList()
        {
            return ((IUserService)DbClient).GetUserList();
        }

        public async Task<UserDTO[]> GetUserListAsync()
        {
            IToken token = new RequestToken();
            await StartUpdateWhenReady(UpdateKind.Loading, token);
            var retVal = await ((IUserService)DbClient).GetUserListAsync();
            EndUpdate(token);
            return retVal;
        }

        public UserDTO PutUserData(UserDTO user, string pw)
        {
            return ((IUserService)DbClient).PutUserData(user, pw);
        }

        public async Task<UserDTO> PutUserDataAsync(UserDTO user, string pw)
        {
            IToken token = new RequestToken();
            await StartUpdateWhenReady(UpdateKind.Updating, token);
            var retVal = await ((IUserService)DbClient).PutUserDataAsync(user, pw);
            EndUpdate(token);
            return retVal;
        }
    }
}
