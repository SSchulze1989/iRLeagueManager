// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.Data;
using iRLeagueManager.Models.Filters;
using iRLeagueManager.Models.Statistics;
using iRLeagueManager.Extensions;

namespace iRLeagueManager.Models
{
    public class ModelManager : IModelManager
    {
        public ModelManager(IModelCache modelCache, IConfigurationProvider mapperConfiguration)
        {
            ModelCache = modelCache;
            MapperConfiguration = mapperConfiguration as MapperConfiguration;
        }
        public IModelCache ModelCache { get; }

        private MapperConfiguration MapperConfiguration { get; }

        public async Task<T> GetModelAsync<T>(params long[] modelId) where T : MappableModel
        {
            return await GetModelAsync<T>(modelId, false);
        }

        public async Task<T> GetModelAsync<T>(long[] modelId, bool update = false, bool reload = false) where T : MappableModel
        {
            return (await GetModelsAsync<T>(new long[][] { modelId }, update, reload)).FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetModelsAsync<T>(IEnumerable<long> modelIds) where T : MappableModel
        {
            return await GetModelsAsync<T>(modelIds.Select(x => new long[] { x }).ToArray());
        }

        public async Task<IEnumerable<T>> GetModelsAsync<T>(IEnumerable<long[]> modelIds = null, bool update = false, bool reload = false) where T : MappableModel
        {
            object[] data = null;
            List<T> modelList = new List<T>();
            List<T> updateList = new List<T>();
            List<long[]> getModelIds = new List<long[]>();

            if (modelIds != null)
            {
                foreach (var modelId in modelIds)
                {
                    var loadedModel = ModelCache.GetModel<T>(modelId.Cast<object>().ToArray());
                    if (loadedModel != null)
                    {
                        modelList.Add(loadedModel);
                        if (reload || loadedModel.IsExpired)
                        {
                            getModelIds.Add(modelId);
                            loadedModel.InitReset();
                        }
                        else
                            updateList.Add(loadedModel);
                    }
                    else
                        getModelIds.Add(modelId);
                }
            }
            else
            {
                getModelIds = null;
            }

            if (update)
            {
                _ = GetModelsAsync<T>(updateList.Where(x => x.ContainsChanges == false).Select(x => x.ModelId).ToArray(), update: false, reload: true);
                _ = UpdateModelsAsync(updateList.ToArray());
            }

            if (getModelIds == null || getModelIds.Count > 0)
            {
                {
                    throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
                }
                //data = await DbContext.GetAsync(getModelIds?.Select(x => x.ToArray()).ToArray(), typeof(T));

                if (data == null)
                {
                    if (getModelIds == null)
                        return null;

                    foreach (var modelId in getModelIds)
                    {
                        var add = await GetModelAsync<T>(modelId.ToArray());
                        if (add == null)
                            return new T[0];
                        if (modelList.Any(x => x != null && x.ModelId.SequenceEqual(add.ModelId)))
                            add.CopyTo(modelList.SingleOrDefault(x => x.ModelId.SequenceEqual(add.ModelId)));
                        else
                            modelList.Add(add);
                    }
                }
                else
                {
                    var mapper = MapperConfiguration.CreateMapper();
                    var addList = new List<T>();
                    var validData = data.Where(x => x != null);
                    //mapper.Map(validData, addList);
                    foreach(var dto in validData)
                    {
                        var add = mapper.Map<T>(dto);
                        addList.Add(add);
                    }
                    //modelList.AddRange(addList);
                    //foreach (var add in addList)
                    //{
                    //    if (modelList.Any(x => x != null && x.ModelId.SequenceEqual(add.ModelId)))
                    //        add.CopyTo(modelList.SingleOrDefault(x => x.ModelId.SequenceEqual(add.ModelId)));
                    //    else
                    //        modelList.Add(add);
                    //}
                    modelList = addList;
                }
            }

            foreach (var model in modelList)
            {
                if (model != null)
                {
                    model.ResetChangedState();
                    model.InitializeModel();
                }
            }

            if (modelIds == null)
                return modelList;
            else
            {
                return modelIds.Select(x => modelList.SingleOrDefault(y => y != null && y.ModelId.SequenceEqual(x)));
            }
        }

        public async Task<T> UpdateModelAsync<T>(T model) where T : MappableModel
        {
            return (await UpdateModelsAsync<T>(new T[] { model })).FirstOrDefault();
        }

        public async Task<IEnumerable<T>> UpdateModelsAsync<T>(IEnumerable<T> models) where T : MappableModel
        {
            List<T> modelList = models.ToList();

            var mapper = MapperConfiguration.CreateMapper();
            object[] data = new object[0];

            if (models == null)
                return null;
            if (models.Count() == 0)
                return modelList;

            foreach (var model in models)
            {
                model.InitReset();
            }

            {
                throw new UnknownModelTypeException("Could not put Model of type " + typeof(T).ToString() + ". Model type not known.");
            }

            if (data == null)
                return null;

            for (int i = 0; i < modelList.Count; i++)
            {
                if (data[i] != null)
                {
                    modelList[i] = mapper.Map<T>(data[i]);
                    modelList[i].ResetChangedState();
                    modelList[i].InitializeModel();
                }
                else
                {
                    modelList[i] = null;
                }
            }

            return modelList;
        }

        public async Task<bool> DeleteModelAsync<T>(params long[] modelId) where T : MappableModel
        {
            return await DeleteModelsAsync<T>(new long[][] { modelId });
        }

        public async Task<bool> DeleteModelsAsync<T>(params T[] models) where T : MappableModel
        {
            if (models.Count() == 0)
                return true;

            return await DeleteModelsAsync<T>(models.Select(x => x.ModelId.ToArray()).ToArray());
        }

        public async Task<bool> DeleteModelsAsync<T>(long[] modelIds) where T : MappableModel
        {
            return await DeleteModelsAsync<T>(modelIds.Select(x => new long[] { x }).ToArray());
        }

        public async Task<bool> DeleteModelsAsync<T>(long[][] modelIds) where T : MappableModel
        {
            if (modelIds != null)
            {
                foreach (var modelId in modelIds)
                {
                    ModelCache.RemoveReference<T>(modelId.Cast<object>().ToArray());
                }
            }

            {
                throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");
            }
        }

        public async Task<T> AddModelAsync<T>(T model) where T : MappableModel
            => (await AddModelsAsync(model)).FirstOrDefault();

        public async Task<IEnumerable<T>> AddModelsAsync<T>(params T[] models) where T : MappableModel
        {
            List<T> modelList = models.ToList();

            var mapper = MapperConfiguration.CreateMapper();
            object[] data = null;

            throw new UnknownModelTypeException("Could not load Model of type " + typeof(T).ToString() + ". Model type not known.");

            for (int i = 0; i < data.Count(); i++)
            {
                if (data[i] != null)
                {
                    modelList[i] = mapper.Map<T>(data[i]);
                    modelList[i].ResetChangedState();
                    modelList[i].InitializeModel();
                }
                else
                {
                    modelList[i] = null;
                }
            }

            return modelList;
        }

        public void ForceExpireModels<T>(IEnumerable<T> models = null) where T : MappableModel
        {
            List<T> expireList;
            if (models != null)
            {
                expireList = models.ToList();
            }
            else
            {
                expireList = ModelCache.GetOfType<T>().ToList();
            }

            expireList.Where(x => x != null).ForEach(x => x.IsExpired = true);
        }

        public void ForceExpireModels<T>(IEnumerable<long[]> modelIds) where T : MappableModel
        {
            ForceExpireModels<T>(ModelCache.GetOfType<T>().Where(x => modelIds.Any(y => y.SequenceEqual(x.ModelId))).ToList());
        }
    }
}
