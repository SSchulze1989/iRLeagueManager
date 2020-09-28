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
