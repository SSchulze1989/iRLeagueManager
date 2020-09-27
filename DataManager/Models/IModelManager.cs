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

        /// <summary>
        /// Forces models that are currently stored in the model cache to expire.
        /// This will make sure, that the models are loaded again from the database when requested next time.
        /// </summary>
        /// <typeparam name="T">Type of Model</typeparam>
        /// <param name="models">Specific model to expire. Default = null for expiring all models of this type.</param>
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
