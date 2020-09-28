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
