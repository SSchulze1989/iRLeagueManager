using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Data;
using iRLeagueManager.Models;

namespace iRLeagueManager.Models
{
    public class ModelCache : IModelCache
    {
        //private readonly LeagueContext leagueContext;

        private readonly ModelDictionary referenceList;

        private readonly Dictionary<Type, ModelRegister> registeredModelTypes;

        private readonly List<KeyValuePair<DateTime, object>> modelBuffer;

        private readonly int bufferKeepMinutes = 5;

        public ModelCache()
        {
            referenceList = new ModelDictionary();
            registeredModelTypes = new Dictionary<Type, ModelRegister>();
            modelBuffer = new List<KeyValuePair<DateTime, object>>();
            //this.leagueContext = leagueContext;
        }

        private ModelIdentifier GetIdentifier(Type type, object[] modelId)
        {
            return new ModelIdentifier(type, modelId);
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
            modelBuffer.RemoveAll(x => x.Key < DateTime.Now.Subtract(TimeSpan.FromMinutes(bufferKeepMinutes)));
        }

        public T GetModel<T>(params object[] modelId) where T : class, ICacheableModel
        {
            CleanReferences();
            var identifier = GetIdentifier(typeof(T), modelId);
            if (referenceList.ContainsKey(identifier))
            {
                T model = referenceList[identifier].Target as T;
                return model;
            }
            return null;
        }

        public void RemoveReference<T>(object[] modelId) where T: class, ICacheableModel
        {
            var identifier = GetIdentifier(typeof(T), modelId);
            referenceList.Remove(identifier);
        }

        public T PutOrGetModel<T>(T model) where T : class, ICacheableModel
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

        public void PutModel<T>(T model) where T : class, ICacheableModel
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

        public void PutModel<T>(ref T model) where T : class, ICacheableModel
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

        public void RegisterModelType<T>(Func<object[], Task<T>> getFunc, Func<T, Task<T>> updateFunc) where T : class, ICacheableModel
        {
            RegisterModelType(typeof(T), async x => await getFunc(x) as ICacheableModel, async x => await updateFunc(x as T) as ICacheableModel);
        }

        public void RegisterModelType(Type type, Func<object[], Task<ICacheableModel>> getFuncAsync, Func<ICacheableModel, Task<ICacheableModel>> updateFuncAsync)
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
