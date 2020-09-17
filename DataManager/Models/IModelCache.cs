using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models
{
    public interface IModelCache<TModel, TKey>
    {
        void CleanReferences();
        T GetModel<T>(params TKey[] modelId) where T : class, TModel;
        T PutOrGetModel<T>(T model) where T : class, TModel;
        void PutModel<T>(T model) where T : class, TModel;
        void PutModel<T>(ref T model) where T : class, TModel;
        void RemoveReference<T>(TKey[] modelId) where T : class, TModel;
        IEnumerable<T> GetOfType<T>() where T : class, TModel;
    }

    public interface IModelCache : IModelCache<ICacheableModel, object> { }
}
