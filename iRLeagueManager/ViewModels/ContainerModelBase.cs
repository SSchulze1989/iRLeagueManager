using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace iRLeagueManager.ViewModels
{
    public abstract class ContainerModelBase<TSource> : ViewModelBase, IDisposable where TSource : class, INotifyPropertyChanged
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

        public ContainerModelBase() { }

        public ContainerModelBase(TSource source)
        {
            Source = source;
        }

        public TSource GetSource()
        {
            return Source;
        }

        protected bool SetSource(TSource source)
        {
            return UpdateSource(source);
        }

        internal virtual bool UpdateSource(TSource source)
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
                OnUpdateSource();

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
