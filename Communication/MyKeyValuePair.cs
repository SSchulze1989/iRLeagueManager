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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace iRLeagueManager
{
    public class MyKeyValuePair : INotifyPropertyChanged
    {
        private object key;
        public object KeyObject { get => key; protected set { key = value; OnPropertyChanged(); } }

        private object value;
        public object ValueObject { get => value; set { this.value = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public MyKeyValuePair(object key, object value)
        {
            KeyObject = key;
            ValueObject = value;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class MyKeyValuePair<TKey, TValue> : MyKeyValuePair, INotifyPropertyChanged
    {
        public TKey Key { get => (TKey)base.KeyObject; set { base.KeyObject = value; } }
        public TValue Value { get => (TValue)base.ValueObject; set { base.ValueObject = value; } }

        public MyKeyValuePair(TKey key, TValue value) : base(key, value) { }
    }
}
