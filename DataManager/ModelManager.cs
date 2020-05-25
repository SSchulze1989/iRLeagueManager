using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Data;
using iRLeagueManager.Models;

namespace iRLeagueManager
{
    public class ModelManager
    {
        //private readonly LeagueContext leagueContext;

        private readonly ModelDictionary referenceList;

        private readonly Dictionary<Type, ModelRegister> registeredModelTypes;

        private readonly List<KeyValuePair<DateTime, object>> modelBuffer;

        public ModelManager()
        {
            referenceList = new ModelDictionary();
            registeredModelTypes = new Dictionary<Type, ModelRegister>();
            modelBuffer = new List<KeyValuePair<DateTime, object>>();
            //this.leagueContext = leagueContext;
        }

        public void CleanReferences()
        {
            var iterator = referenceList.ToList();

            foreach(var reference in iterator)
            {
                if (!reference.Value.IsAlive)
                {
                    referenceList.Remove(reference.Key);
                }
            }
            modelBuffer.RemoveAll(x => x.Key < DateTime.Now.Subtract(TimeSpan.FromMinutes(1)));
        }

        public T GetModel<T>(params long[] modelId) where T : ModelBase
        {
            CleanReferences();
            var identifier = new ModelIdentifier(typeof(T), modelId);
            if (referenceList.ContainsKey(identifier))
            {
                T model = referenceList[identifier].Target as T;
                return model;
            }
            return null;
        }

        public T PutOrGetModel<T>(T model) where T : ModelBase
        {
            CleanReferences();
            var getModel = GetModel<T>(model.ModelId);
            if (getModel != null)
            {
                return getModel;
            }
            else
            {
                referenceList.Add(model);
                modelBuffer.Add(new KeyValuePair<DateTime, object>(DateTime.Now, model));
                return model;
            }
        }

        public void PutModel<T>(T model) where T : ModelBase
        {
            CleanReferences();
            var identfier = new ModelIdentifier(model);
            if (referenceList.ContainsKey(identfier))
            {
                throw new ArgumentException("Model is already registered in Model Manager! Consider using this function with \"ref\" keyword to prevent this Error.");
            }
            else
            {
                referenceList.Add(model);
                modelBuffer.Add(new KeyValuePair<DateTime, object>(DateTime.Now, model));
            }
        }

        public void PutModel<T>(ref T model) where T : ModelBase
        {
            CleanReferences();
            var identfier = new ModelIdentifier(model);
            if (referenceList.ContainsKey(identfier))
            {
                model = referenceList[identfier].Target as T;
            }
            else
            {
                referenceList.Add(model);
                modelBuffer.Add(new KeyValuePair<DateTime, object>(DateTime.Now, model));
            }
        }

        public void RegisterModelType<T>(Func<long[], Task<T>> getFunc, Func<T, Task<T>> updateFunc) where T : ModelBase
        {
            RegisterModelType(typeof(T), async x => await getFunc(x) as ModelBase, async x => await updateFunc(x as T) as ModelBase);
        }

        public void RegisterModelType(Type type, Func<long[], Task<ModelBase>> getFuncAsync, Func<ModelBase, Task<ModelBase>> updateFuncAsync)
        {
            if (registeredModelTypes.ContainsKey(type))
            {
                throw new InvalidOperationException("Could not register Model type. Model already registered");
            }
            else
            {
                registeredModelTypes.Add(type, new ModelRegister(type, getFuncAsync, updateFuncAsync));
            }
        }
    }
}
