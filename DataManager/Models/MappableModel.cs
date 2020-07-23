using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Collections;

namespace iRLeagueManager.Models
{
    public abstract class MappableModel : VersionModel, INotifyPropertyChanged, ICacheableModel
    {
        public abstract long[] ModelId { get; }

        object[] ICacheableModel.ModelId => ModelId?.Select(x => (object)x).ToArray();

        public MappableModel()
        {
            isInitialized = false;
        }

        public virtual bool CompareIdentity(ICacheableModel comp)
        {
            return this.ModelId.SequenceEqual(comp.ModelId.Cast<long>());
        }

        public virtual bool IsBaseType()
        {
            return this.GetType().Equals(typeof(MappableModel));
        }

        public virtual Type GetBaseType()
        {
            return typeof(MappableModel);
        }
    }
}
