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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models;

namespace iRLeagueManager
{
    public class ModelDictionary : Dictionary<IModelIdentifier, WeakReference>
    {
        public void Add(ICacheableModel model)
        {
            Type type = model.GetType();
            var reference = new WeakReference(model);

            while (!type.Equals(model.GetBaseType()))
            {
                try
                {
                    var key = new ModelIdentifier(type, model.ModelId);
                    if (this.ContainsKey(key))
                        Replace(key, reference);
                    else
                        Add(key, reference);

                    type = type.BaseType;
                }
                catch (Exception e)
                {
                    throw new AddModelDictionaryItemException(type, model.ModelId, e);
                }
            }
        }

        public void AddRange(IEnumerable<KeyValuePair<IModelIdentifier, WeakReference>> keyValuePairs)
        {
            foreach(var pair in keyValuePairs)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public void AddRange(IEnumerable<ICacheableModel> models)
        {
            foreach(var model in models)
            {
                Add(model);
            }
        }

        private void Replace(IModelIdentifier key, WeakReference value)
        {
            if (this.ContainsKey(key))
            {
                this[key] = value;
            }
        }
    }

    public class AddModelDictionaryItemException : Exception
    {
        public Type ModelType { get; set; }
        public object[] ModelId { get; set; }

        public AddModelDictionaryItemException(Type modelType, object[] modelId, Exception innerException = null) : this(modelType, modelId, 
            $"Error while adding Item to ModelDictionary -> Type: {modelType.ToString()} | " +
            $"modelId: {modelId.Select(x => x.ToString()).Aggregate((x,y) => $"{x}, {y}")}.", innerException)
        { }

        public AddModelDictionaryItemException(Type modelType, object[] modelId, string message) : base(message)
        {
            ModelType = modelType;
            ModelId = modelId;
        }

        public AddModelDictionaryItemException(Type modelType, object[] modelId, string message, Exception innerException) : base(message, innerException)
        {
            ModelType = modelType;
            ModelId = modelId;
        }

        protected AddModelDictionaryItemException(Type modelType, object[] modelId, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ModelType = modelType;
            ModelId = modelId;
        }
    }
}
