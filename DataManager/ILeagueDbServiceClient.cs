using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Enums;
using iRLeagueManager.LeagueDBServiceRef;
using iRLeagueManager.UserDBServiceRef;

namespace iRLeagueManager.Data
{
    public interface ILeagueDbServiceClient : IDisposable, INotifyPropertyChanged
    {
        string ServiceAddress { get; }
        ConnectionStatusEnum ConnectionStatus { get; }

        bool OpenConnection();
        IClientUser Register(string userName, string password);
        ICustomAuthenticationResult Authenticate(string userName, string password);
        bool CloseConnection();

        TTarget[] Get<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO;
        Task<TTarget[]> GetAsync<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO;
        TTarget[] Post<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO;
        Task<TTarget[]> PostAsync<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO;
        TTarget[] Put<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO;
        Task<TTarget[]> PutAsync<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO;
        bool Del<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO;
        Task<bool> DelAsync<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO;
    }
}
