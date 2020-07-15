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
        T GetModel<T>(params TKey[] modelId) where T : TModel;
        T PutOrGetModel<T>(T model) where T : TModel;
        void PutModel<T>(T model) where T : TModel;
        void PutModel<T>(ref T model) where T : TModel;
    }

    public interface IModelCache : IModelCache<ModelBase, long> { }
}
