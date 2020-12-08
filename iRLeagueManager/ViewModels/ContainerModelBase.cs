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
using System.ComponentModel;

namespace iRLeagueManager.ViewModels
{
    public abstract class ContainerModelBase<TSource> : ViewModelBase, IContainerModelBase<TSource>, IDisposable where TSource : class, INotifyPropertyChanged
    {
        private TSource _source;
        //[NotifyParentProperty(true)]
        protected TSource Source
        {
            get => _source;
            private set
            {
                if (SetValue(ref _source, value))
                    OnPropertyChanged(null);
            }
        }

        public event Action ModelChanged;

        public ContainerModelBase() { }

        public ContainerModelBase(TSource source)
        {
            Source = source;
        }

        ~ContainerModelBase()
        {
            Dispose(false);
        }

        public TSource GetSource()
        {
            return Source;
        }

        protected bool SetSource(TSource source)
        {
            return UpdateSource(source);
        }

        public virtual bool UpdateSource(TSource source)
        {
            if (_source != null)
            {
                _source.PropertyChanged -= this.FwdPropertyChanged;
            }
            bool hasChanged = SetValue(ref this._source, source);
            if (_source != null)
            {
                _source.PropertyChanged += this.FwdPropertyChanged;
            }

            OnPropertyChanged(null);

            if (hasChanged)
            {
                OnUpdateSource();
                ModelChanged?.Invoke();
            }

            return hasChanged;
        }

        void FwdPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public virtual void OnUpdateSource() { }

        protected override void Dispose(bool disposing)
        {
            if (_source != null)
            {
                _source.PropertyChanged -= this.FwdPropertyChanged;
            }
            _source = null;
        }
    }
}
