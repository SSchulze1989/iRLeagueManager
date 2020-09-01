using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueDatabase.DataTransfer;

namespace iRLeagueManager.Data
{
    public interface IModelDataProvider<TModelDTO, TKey> : IDisposable
    {
        Task<TModelDTO[]> GetAsync(TKey[][] requestIds, Type requestType);
        Task<TTarget[]> GetAsync<TTarget>(TKey[][] requestIds) where TTarget : TModelDTO;
        Task<TModelDTO[]> PostAsync(TModelDTO[] items, Type requestType);
        Task<TTarget[]> PostAsync<TTarget>(TTarget[] items) where TTarget : TModelDTO;
        Task<TModelDTO[]> PutAsync(TModelDTO[] items, Type requestType);
        Task<TTarget[]> PutAsync<TTarget>(TTarget[] items) where TTarget : TModelDTO;
        Task<bool> DeleteAsync(TKey[][] requestIds, Type requestType);
        Task<bool> DelAsync<TTarget>(TKey[][] requestIds) where TTarget : TModelDTO;
    }

    public interface IModelDataProvider :  IModelDataProvider<MappableDTO, long> { }
}
