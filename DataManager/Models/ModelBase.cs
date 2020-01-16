using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace iRLeagueManager.Models
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        protected bool isInitialized;

        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime? createdOn;
        public DateTime? CreatedOn { get => createdOn; internal set { createdOn = value; OnPropertyChanged(); } }

        private DateTime? lastModifiedOn;
        public DateTime? LastModifiedOn { get => lastModifiedOn; internal set { lastModifiedOn = value; OnPropertyChanged(); } }

        private int version;
        public int Version { get => version; internal set { version = value; OnPropertyChanged(); } }

        public ModelBase()
        {
            CreatedOn = DateTime.Now;
            lastModifiedOn = DateTime.Now;
            isInitialized = false;
        }

        public virtual void CopyTo(ModelBase targetObject)
        {
            Type sourceType = this.GetType();
            if (!targetObject.GetType().Equals(sourceType))
                return;

            targetObject.InitReset();

            foreach(var property in sourceType.GetProperties())
            {
                if (property.GetMethod == null || property.SetMethod == null)
                    continue;

                property.SetValue(targetObject, property.GetValue(this));
            }

            if (isInitialized)
                targetObject.InitializeModel();
        }

        public virtual void CopyFrom(ModelBase sourceObject)
        {
            Type targetType = this.GetType();
            if (!sourceObject.GetType().Equals(targetType))
                return;

            InitReset();

            foreach(var property in targetType.GetProperties())
            {
                if (property.GetMethod == null || property.SetMethod == null)
                    continue;

                property.SetValue(this, property.GetValue(sourceObject));
            }

            if (sourceObject.isInitialized)
                InitializeModel();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetValue<T>(ref T targetProperty, T value, [CallerMemberName] string propertyName = "")
        {
            if (targetProperty == null)
            {
                if (value != null)
                {
                    targetProperty = value;
                    if (isInitialized)
                    {
                        LastModifiedOn = DateTime.Now;
                    }
                    OnPropertyChanged(propertyName);
                    return true;
                }
                return false;
            }

            if (!targetProperty.Equals(value))
            {
                targetProperty = value;
                if (isInitialized)
                {
                    LastModifiedOn = DateTime.Now;
                }
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        protected bool SetNotifyCollection<T>(ref T targetCollection, T value, [CallerMemberName] string propertyName ="") where T : INotifyCollectionChanged
        {
            var last = targetCollection;
            bool changed = SetValue(ref targetCollection, value);

            if (changed)
            {
                if (last != null)
                    last.CollectionChanged -= OnCollectionChanged;

                if (targetCollection != null)
                    targetCollection.CollectionChanged += OnCollectionChanged;
            }

            return changed;
        }

        protected void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (isInitialized)
                LastModifiedOn = DateTime.Now;
        }

        internal virtual void InitReset()
        {
            isInitialized = false;
        }

        internal virtual void InitializeModel()
        {
            isInitialized = true;
        }
    }
}
