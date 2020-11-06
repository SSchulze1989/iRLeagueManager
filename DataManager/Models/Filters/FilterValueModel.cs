using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Filters
{
    public class FilterValueModel : ModelBase
    {
        private Type valueType;
        public Type ValueType { get => valueType; set => SetValue(ref valueType, value); }

        private object _value;
        public object Value { get => _value; set => SetValue(ref _value, value); }

        public FilterValueModel() { }

        public FilterValueModel(Type valueType, object value)
        {
            ValueType = valueType;
            Value = value;
        }
    }
}
