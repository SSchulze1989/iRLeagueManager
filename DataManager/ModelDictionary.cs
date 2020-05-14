using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models;

namespace iRLeagueManager
{
    public class ModelDictionary : Dictionary<IModelIdentifier, WeakReference>
    {
        public void Add(ModelBase model)
        {
            Type type = model.GetType();

            while (!type.Equals(typeof(ModelBase)))
            {
                Add(new ModelIdentifier(type, model.ModelId), new WeakReference(model));
                type = type.BaseType;
            }
        }

        public void AddRange(IEnumerable<KeyValuePair<IModelIdentifier, WeakReference>> keyValuePairs)
        {
            foreach(var pair in keyValuePairs)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public void AddRange(IEnumerable<ModelBase> models)
        {
            foreach(var model in models)
            {
                Add(model);
            }
        }
    }
}
