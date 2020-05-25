using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace iRLeagueManager
{
    public class MyKeyValuePair : INotifyPropertyChanged
    {
        private object key;
        public object Key { get => key; protected set { key = value; OnPropertyChanged(); } }

        private object value;
        public object Value { get => value; set { this.value = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public MyKeyValuePair(object key, object value)
        {
            Key = key;
            Value = value;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class MyKeyValuePair<TKey, TValue> : MyKeyValuePair, INotifyPropertyChanged
    {
        public new TKey Key { get => (TKey)base.Key; protected set { base.Key = value; } }
        public new TValue Value { get => (TValue)base.Value; set { base.Value = value; } }

        public MyKeyValuePair(TKey key, TValue value) : base(key, value) { }
    }
}
