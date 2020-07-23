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
