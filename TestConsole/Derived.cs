using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class Derived : Base
    {
        private string derivedProperty;
        public string DerivedProperty { get => derivedProperty; set { derivedProperty = value; OnPropertyChanged(); } }
    }
}
