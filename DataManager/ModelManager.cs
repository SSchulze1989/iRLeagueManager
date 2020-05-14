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

        public ModelManager()
        {
            referenceList = new ModelDictionary();
            registeredModelTypes = new Dictionary<Type, ModelRegister>();
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
            }
        }

        //public async Task<T> GetModelAsync<T>(params long[] modelId) where T : ModelBase
        //{
        //    CleanReferences();
        //    var identifier = new ModelIdentifier(typeof(T), modelId);
        //    if (referenceList.ContainsKey(identifier))
        //    {
        //        T model = referenceList[identifier].Target as T;
        //        //leagueContext.UpdateModelAsync(model);
        //        return model;
        //    }
        //    else
        //    {
        //        T model = await leagueContext.GetModelAsync<T>(modelId);
        //        referenceList.Add(identifier, new WeakReference(model));
        //        return model;
        //    }
        //}

        //public async Task<IEnumerable<T>> GetModelsAsync<T>(long[] modelIds) where T : ModelBase
        //{
        //    return await GetModelsAsync<T>(modelIds.Select(x => new long[] { x }).ToArray());
        //}

        //public async Task<IEnumerable<T>> GetModelsAsync<T>(long[][] modelIds = null) where T : ModelBase
        //{
        //    CleanReferences();
        //    var modelList = new List<T>();
        //    var loadModelIds = new List<long[]>();

        //    if (modelIds != null && modelIds.Count() > 0)
        //    {
        //        foreach (var modelId in modelIds)
        //        {
        //            var identifier = new ModelIdentifier(typeof(T), modelId);

        //            if (referenceList.ContainsKey(identifier))
        //            {
        //                T model = referenceList[identifier].Target as T;
        //                modelList.Add(model);
        //            }
        //            else
        //            {
        //                loadModelIds.Add(modelId);
        //            }
        //        }

        //        //await leagueContext.UpdateModelsAsync(modelList.ToList());
        //        if (loadModelIds.Count > 0)
        //        {
        //            var addList = await leagueContext.GetModelsAsync<T>(loadModelIds);
        //            modelList.AddRange(addList);
        //            referenceList.AddRange(addList);
        //        }
        //    }
        //    else
        //    {
        //        var updateList = new List<T>();
        //        modelList.AddRange(await leagueContext.GetModelsAsync<T>());

        //        foreach(var model in modelList.ToList())
        //        {
        //            var identifier = new ModelIdentifier(model);

        //            if (referenceList.ContainsKey(identifier))
        //            {
        //                var index = modelList.IndexOf(model);
        //                var loadedModel = referenceList[identifier].Target as T;
        //                modelList.RemoveAt(index);
        //                modelList.Insert(index, loadedModel);
        //                updateList.Add(loadedModel);
        //            }
        //            else
        //            {
        //                referenceList.Add(model);
        //            }
        //        }
        //        if (updateList.Count > 0)
        //        {
        //            //leagueContext.UpdateModelsAsync(updateList);
        //        }
        //    }

        //    return modelList;
        //}

        //public async Task<T> UpdateModelAsync<T>(T model) where T : ModelBase
        //{
        //    return await leagueContext.UpdateModelAsync(model);
        //}

        //public async Task<IEnumerable<T>> UpdateModelsAsync<T>(IEnumerable<T> models) where T : ModelBase
        //{
        //    return await leagueContext.UpdateModelsAsync(models);
        //}

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

        //public async Task<T> GetModel<T>(params long[] modelId) where T : ModelBase
        //{
        //    var identifier = new ModelIdentifier(typeof(T), modelId);

        //    if (registeredModelTypes.ContainsKey(typeof(T)))
        //    {
        //        var typeRegister = registeredModelTypes[typeof(T)];
        //        if (referenceList.ContainsKey(identifier))
        //        {
        //            T model = referenceList[identifier].Target as T;
        //            typeRegister.UpdateModelAsync(model);
        //            return model;
        //        }
        //        else
        //        {
        //            T model = await typeRegister.GetModelAsync(modelId) as T;
        //            referenceList.Add(identifier, new WeakReference(model));
        //            return model;
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Model type " + typeof(T).Name + " not registered.");
        //    }
        //}

        //public async Task<T> UpdateModelAsync<T>(T model) where T : ModelBase
        //{
        //    if (registeredModelTypes.ContainsKey(typeof(T)))
        //    {
        //        var typeRegister = registeredModelTypes[typeof(T)];
        //        return await typeRegister.UpdateModelAsync(model) as T;
        //    }
        //    else
        //    {
        //        throw new Exception("Model type " + typeof(T).Name + " not registered.");
        //    }
        //}
    }
}
