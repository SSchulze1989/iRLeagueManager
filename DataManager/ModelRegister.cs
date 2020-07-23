using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models;

namespace iRLeagueManager
{
    public class ModelRegister
    {
        private readonly Type modelType;

        private readonly Func<object[], Task<ICacheableModel>> getFuncAsync;

        private readonly Func<ICacheableModel, Task<ICacheableModel>> updateFuncAsync;

        //private readonly Action<ModelBase> saveFunc;

        public ModelRegister(Type type, Func<object[], Task<ICacheableModel>> getFunc, Func<ICacheableModel, Task<ICacheableModel>> updateFunc)
        {
            modelType = type;
            this.getFuncAsync = getFunc;
            this.updateFuncAsync = updateFunc;
        }

        public async Task<ICacheableModel> GetModelAsync(object[] modelId)
        {
            return await getFuncAsync(modelId);
        }

        public async Task<ICacheableModel> UpdateModelAsync(ICacheableModel model)
        {
            return await updateFuncAsync(model);
        }
    }
}
