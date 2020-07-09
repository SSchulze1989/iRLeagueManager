using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Enums;
using iRLeagueManager.LeagueDBServiceRef;
using iRLeagueManager.Models;
using iRLeagueManager.UserDBServiceRef;

namespace iRLeagueManager.Data
{
    public interface ILeagueDbServiceClient : IDisposable, INotifyPropertyChanged
    {
        string ServiceAddress { get; }
        ConnectionStatusEnum ConnectionStatus { get; }

        bool OpenConnection();
        LeagueUserDTO RegisterUser(string userName, byte[] password, string databaseName);
        Task<LeagueUserDTO> RegisterUserAsync(string userName, byte[] password, string databaseName);
        AuthenticationResult AuthenticateUser(string userName, byte[] password, string databaseName);
        Task<AuthenticationResult> AuthenticateUserAsync(string userName, byte[] password, string databaseName);
        bool CloseConnection();

        //MappableDTO[] Get(long[][] requestIds, Type requestType, string userName, string password, string databaseName);
        //TTarget[] Get<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO;
        Task<MappableDTO[]> GetAsync(long[][] requestIds, Type requestType, string databaseName);
        Task<TTarget[]> GetAsync<TTarget>(long[][] requestIds, string databaseName) where TTarget : MappableDTO;
        //TTarget[] Post<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO;
        Task<MappableDTO[]> PostAsync(MappableDTO[] items, Type requestType, string databaseName);
        Task<TTarget[]> PostAsync<TTarget>(TTarget[] items, string databaseName) where TTarget : MappableDTO;
        //TTarget[] Put<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO;
        Task<MappableDTO[]> PutAsync(MappableDTO[] items, Type requestType, string databaseName);
        Task<TTarget[]> PutAsync<TTarget>(TTarget[] items, string databaseName) where TTarget : MappableDTO;
        //bool Del<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO;
        Task<bool> DelAsync(long[][] requestIds, Type requestType, string databaseName);
        Task<bool> DelAsync<TTarget>(long[][] requestIds, string databaseName) where TTarget : MappableDTO;
    }
}
