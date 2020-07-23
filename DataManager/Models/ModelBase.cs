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
    public class ModelBase : NotifyPropertyChangedBase
    {
        protected bool isInitialized;

        private bool isReadOnly;
        public bool IsReadOnly { get => isReadOnly; internal set => SetValue(ref isReadOnly, value); }

        public ModelBase() { }

        public virtual void CopyTo(ModelBase targetObject)
        {
            Type sourceType = this.GetType();
            Type targetType = targetObject.GetType();

            if (!(targetType.Equals(sourceType) || targetType.IsSubclassOf(sourceType) || sourceType.IsSubclassOf(targetType)))
                return;

            targetObject.InitReset();

            foreach (var property in targetType.GetProperties())
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
            Type sourceType = sourceObject.GetType();

            if (!(sourceType.Equals(targetType) || sourceType.IsSubclassOf(targetType) || targetType.IsSubclassOf(sourceType)))
                return;

            InitReset();

            foreach (var property in targetType.GetProperties())
            {
                if (property.GetMethod == null || property.SetMethod == null)
                    continue;

                property.SetValue(this, property.GetValue(sourceObject));
            }

            if (sourceObject.isInitialized)
                InitializeModel();
        }

        protected virtual bool SetValue<T>(ref T targetProperty, T value, [CallerMemberName] string propertyName = "")
        {
            if (targetProperty == null)
            {
                if (value != null)
                {
                    targetProperty = value;
                    if (isInitialized)
                    {
                        //LastModifiedOn = DateTime.Now;
                        //ContainsChanges = true;
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
                    //LastModifiedOn = DateTime.Now;
                    //ContainsChanges = true;
                }
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Setter Method for mapping a collection property that implements INotifyCollectionChanged to a backing field and 
        /// to register the Collection for the OnCollectionChanged handler; This is Required for LastModifiedOn and LastModifiedBy to be updated
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetCollection"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual bool SetNotifyCollection<T>(ref T targetCollection, T value, [CallerMemberName] string propertyName = "") where T : INotifyCollectionChanged
        {
            var last = targetCollection;
            bool changed = SetValue(ref targetCollection, value, propertyName);

            if (changed)
            {
                if (last != null)
                    last.CollectionChanged -= OnCollectionChanged;

                if (targetCollection != null)
                    targetCollection.CollectionChanged += OnCollectionChanged;
            }

            return changed;
        }

        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (isInitialized)
            {
                //LastModifiedOn = DateTime.Now;
                //ContainsChanges = true;
            }
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
