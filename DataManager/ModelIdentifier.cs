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
    class ModelIdentifier : IModelIdentifier, IEquatable<IModelIdentifier>
    {
        public Type ModelType { get; }
        public object[] ModelId { get; }

        public ModelIdentifier(ICacheableModel model)
        {
            ModelType = model.GetType();
            ModelId = model.ModelId;
        }

        public ModelIdentifier(Type type, object[] id)
        {
            ModelType = type;
            ModelId = id;
        }

        public override bool Equals(object obj)
        {
            if (obj is IModelIdentifier identifier)
                return Equals(identifier);
            return false;
        }

        public bool Equals(IModelIdentifier other)
        {
            bool ret = true;
            ret &= !(other == null);
            ret &= ModelType.Equals(other.ModelType);
            ret &= ModelId.SequenceEqual(ModelId);
            return ret;
        }

        public bool Equals(ModelIdentifier other)
        {
            return Equals(other as IModelIdentifier);
        }

        public override int GetHashCode()
        {
            var hashCode = 557061185;
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(ModelType);
            //hashCode = hashCode * -1521134295 + EqualityComparer<long[]>.Default.GetHashCode(modelId);
            foreach(var id in ModelId)
            {
                hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(id);
            }
            return hashCode;
        }

        public int GetHashCode(ICacheableModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
