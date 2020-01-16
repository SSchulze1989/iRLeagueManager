using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestConsole
{
    public class Container : INotifyPropertyChanged
    {
        private Derived derived;
        public Derived Derived
        {
            get => derived;
            set
            {
                if (derived != null)
                {
                    derived.PropertyChanged -= this.NotifyPropertyChanged;
                }
                derived = value;
                if (derived != null)
                {
                    derived.PropertyChanged += this.NotifyPropertyChanged;
                }
            }
        }

        public string Property => Derived.Property;
        public string DerivedProperty => DerivedProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
    }
}
