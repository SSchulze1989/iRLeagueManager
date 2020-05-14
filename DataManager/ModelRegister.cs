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

        private readonly Func<long[], Task<ModelBase>> getFuncAsync;

        private readonly Func<ModelBase, Task<ModelBase>> updateFuncAsync;

        //private readonly Action<ModelBase> saveFunc;

        public ModelRegister(Type type, Func<long[], Task<ModelBase>> getFunc, Func<ModelBase, Task<ModelBase>> updateFunc)
        {
            modelType = type;
            this.getFuncAsync = getFunc;
            this.updateFuncAsync = updateFunc;
        }

        public async Task<ModelBase> GetModelAsync(long[] modelId)
        {
            return await getFuncAsync(modelId);
        }

        public async Task<ModelBase> UpdateModelAsync(ModelBase model)
        {
            return await updateFuncAsync(model);
        }
    }
}
