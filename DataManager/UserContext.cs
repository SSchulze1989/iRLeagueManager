using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.UserDBServiceRef;
using iRLeagueManager.Enums;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Database;
using AutoMapper;
using AutoMapper.EquivalencyExpression;

using iRLeagueManager.Data;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.User
{
    public class UserContext
    {
        public UserModel CurrentUser { get; private set; }

        private ModelMapperProfile ModelMapperProfile { get; }

        private MapperConfiguration MapperConfiguration { get; }

        public DbUserServiceClient UserServiceContext { get; }

        //public DatabaseStatusModel Status => UserServiceContext?.Status;

        public UserContext(IDatabaseStatus status)
        {
            ModelMapperProfile = new ModelMapperProfile();
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(ModelMapperProfile);
                cfg.AddCollectionMappers();
            });
            UserServiceContext = new DbUserServiceClient(status);
        }

        public async Task<bool> UserLoginAsync(string userName, string pw)
        {
            var mapper = MapperConfiguration.CreateMapper();
            var user = await ((IUserService)UserServiceContext).UserLoginAsync(userName, pw);

            if (user == null)
                return false;

            if (CurrentUser == null)
            {
                CurrentUser = new UserModel(user.UserId);
            }

            mapper.Map(user, CurrentUser);
            return true;
        }

        public void UserLogout()
        {
            CurrentUser = null;
        }

        public async Task<bool> UserSetPwAsync(int userId, string oldPw, string newPw)
        {
            return await ((IUserService)UserServiceContext).UserSetPwAsync(userId, oldPw, newPw);
        }

        public bool IsUserNameAvailable(string username)
        {
            return ((IUserService)UserServiceContext).IsUserNameAvailable(username);
        }

        public Task<bool> IsUserNameAvailableAsync(string username)
        {
            return ((IUserService)UserServiceContext).IsUserNameAvailableAsync(username);
        }

        public UserModel CreateUser(string userName, string initialPw)
        {
            var mapper = MapperConfiguration.CreateMapper();
            var userDTO = ((IUserService)UserServiceContext).CreateUser(userName, initialPw);
            return mapper.Map<UserModel>(userDTO);
        }

        public async Task<UserModel> CreateUserAsync(string userName, string initialPw)
        {
            var mapper = MapperConfiguration.CreateMapper();
            var userDTO = await ((IUserService)UserServiceContext).CreateUserAsync(userName, initialPw);
            return mapper.Map<UserModel>(userDTO);
        }

        public async Task<UserModel> CreateUserAsync(UserModel user, string initialPw)
        {
            var userDTO = await ((IUserService)UserServiceContext).CreateUserAsync(user.UserName, initialPw);
            var mapper = MapperConfiguration.CreateMapper();
            var userId = userDTO.UserId;
            mapper.Map(user, userDTO);
            userDTO.UserId = userId;
            userDTO = await ((IUserService)UserServiceContext).PutUserDataAsync(userDTO, initialPw);
            return mapper.Map<UserModel>(userDTO);
        }

        public async Task<UserModel> PutUserDataAsync(UserModel user, string pw)
        {
            var mapper = MapperConfiguration.CreateMapper();
            var userDTO = mapper.Map<UserDTO>(user);
            userDTO = await ((IUserService)UserServiceContext).PutUserDataAsync(userDTO, pw);
            return mapper.Map(userDTO, user);
        }

        //public UserDTO SetAdminRights(UserDTO user, AdminRights rights)
        //{
        //    return ((IUserService)UserServiceContext).SetAdminRights(user, rights);
        //}

        //public Task<UserDTO> SetAdminRightsAsync(UserDTO user, AdminRights rights)
        //{
        //    return ((IUserService)UserServiceContext).SetAdminRightsAsync(user, rights);
        //}

        //public UserDTO GetUserData(int userId)
        //{
        //    return ((IUserService)UserServiceContext).GetUserData(userId);
        //}

        public async Task<UserModel> GetUserDataAsync(int userId)
        {
            var mapper = MapperConfiguration.CreateMapper();
            var userDTO = await ((IUserService)UserServiceContext).GetUserDataAsync(userId);
            return mapper.Map<UserModel>(userDTO);
        }

        public UserModel[] GetUserList()
        {
            var mapper = MapperConfiguration.CreateMapper();
            var userDTOs = ((IUserService)UserServiceContext).GetUserList();
            var userModels = userDTOs.Select(x => mapper.Map<UserModel>(x)).ToArray();
            return userModels;
        }

        public async Task<UserModel[]> GetUserListAsync()
        {
            var mapper = MapperConfiguration.CreateMapper();
            var userDTOs = await ((IUserService)UserServiceContext).GetUserListAsync();
            return userDTOs.Select(x => mapper.Map<UserModel>(x)).ToArray();
        }
    }
}
