using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models
{
    public interface IModelManager<TModel, TKey> 
    {
        IModelCache ModelCache { get; }
        void ForceExpireModels<T>(IEnumerable<T> models = null) where T : TModel;
        Task<T> GetModelAsync<T>(params TKey[] modelId) where T : TModel;
        Task<T> GetModelAsync<T>(TKey[] modelId, bool update = true, bool reload = false) where T : TModel;
        Task<IEnumerable<T>> GetModelsAsync<T>(IEnumerable<TKey> modelIds) where T : TModel;
        Task<IEnumerable<T>> GetModelsAsync<T>(IEnumerable<TKey[]> modelIds = null, bool update = true, bool reload = false) where T : TModel;
        Task<T> UpdateModelAsync<T>(T model) where T : TModel;
        Task<IEnumerable<T>> UpdateModelsAsync<T>(IEnumerable<T> models) where T : TModel;
        Task<bool> DeleteModelAsync<T>(params TKey[] modelId) where T : TModel;
        Task<bool> DeleteModelsAsync<T>(params T[] models) where T : TModel;
        Task<bool> DeleteModelsAsync<T>(TKey[] modelIds) where T : TModel;
        Task<bool> DeleteModelsAsync<T>(TKey[][] modelIds) where T : TModel;
        Task<T> AddModelAsync<T>(T model) where T : TModel;
        Task<IEnumerable<T>> AddModelsAsync<T>(params T[] models) where T : TModel;
    }

    public interface IModelManager : IModelManager<MappableModel, long> { }
}
