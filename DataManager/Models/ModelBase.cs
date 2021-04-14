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
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Collections;
using iRLeagueDatabase.Extensions;
using System.Reflection;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.Models
{
    public class ModelBase : NotifyPropertyChangedBase
    {
        protected bool isInitialized;

        private bool isReadOnly;
        public bool IsReadOnly { get => isReadOnly; internal set => SetValue(ref isReadOnly, value); }

        public ModelBase() 
        {
            isInitialized = false;
        }

        public virtual void CopyTo(ModelBase targetObject, params string[] excludeProperties)
        {
            Type sourceType = this.GetType();
            Type targetType = targetObject.GetType();

            if (!(targetType.Equals(sourceType) || targetType.IsSubclassOf(sourceType) || sourceType.IsSubclassOf(targetType)))
                return;

            targetObject.InitReset();

            foreach (var property in targetType.GetProperties())
            {
                if (excludeProperties.Contains(property.Name))
                    continue;
                if (property.GetMethod == null || property.SetMethod == null)
                    continue;

                // Deep copy models if type is Subclass of ModelBase
                if (typeof(ModelBase).IsAssignableFrom(property.PropertyType) && property.GetCustomAttribute(typeof(DeepCopyModelAttribute)) != null)
                {
                    ModelBase sourceValue = (ModelBase)property.GetValue(this);
                    ModelBase targetValue = (ModelBase)property.GetValue(targetObject);
                    if (sourceValue == null)
                    {
                        targetValue = null;
                    }
                    else
                    {
                        if (targetValue == null)
                        {
                            var constructor = property.PropertyType.GetConstructor(new Type[0]);
                            targetValue = (ModelBase)constructor.Invoke(new object[0]);
                        }
                        sourceValue.CopyTo(targetValue);
                    }
                }
                else if (property.PropertyType.IsGenericType && property.PropertyType.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(ICollection<>)))
                {
                    var interfaceType = property.PropertyType.GetInterfaces().SingleOrDefault(x => x == typeof(ICollection<>));
                    // If target type implements ICollection use target collection instead of overwriting property
                    dynamic targetCollection = property.GetValue(targetObject);
                    IEnumerable sourceCollection = property.GetValue(this) as IEnumerable;
                    if (targetCollection == null || sourceCollection == null)
                    {
                        property.SetValue(targetObject, sourceCollection);
                    }
                    else
                    {
                        var itemType = property.PropertyType.GetGenericArguments().First();
                        var constructor = itemType.GetConstructor(new Type[0]);
                        targetCollection.Clear();
                        foreach (var item in sourceCollection)
                        {
                            if (typeof(ModelBase).IsAssignableFrom(itemType) && property.GetCustomAttribute(typeof(DeepCopyModelAttribute)) != null)
                            {
                                dynamic newItem = (ModelBase)constructor.Invoke(new object[0]);
                                ((ModelBase)item).CopyTo((ModelBase)newItem);
                                targetCollection.Add(newItem);
                            }
                            else
                            {
                                targetCollection.Add(item);
                            }
                        }
                    }
                }
                else
                { 
                    property.SetValue(targetObject, property.GetValue(this)); 
                }
            }

            if (isInitialized)
                targetObject.InitializeModel();
        }

        public virtual void CopyFrom(ModelBase sourceObject, params string[] excludeProperties)
        {
            Type targetType = this.GetType();
            Type sourceType = sourceObject.GetType();

            if (!(sourceType.Equals(targetType) || sourceType.IsSubclassOf(targetType) || targetType.IsSubclassOf(sourceType)))
                return;

            InitReset();

            foreach (var property in targetType.GetProperties())
            {
                if (excludeProperties.Contains(property.Name))
                    continue;
                if (property.GetMethod == null || property.SetMethod == null)
                    continue;

                // Deep copy models if type is Subclass of ModelBase
                if (typeof(ModelBase).IsAssignableFrom(property.PropertyType) && property.GetCustomAttribute(typeof(DeepCopyModelAttribute)) != null)
                {
                    ModelBase sourceValue = (ModelBase)property.GetValue(sourceObject);
                    ModelBase targetValue = (ModelBase)property.GetValue(this);
                    if (sourceValue == null)
                    {
                        targetValue = null;
                    }
                    else
                    {
                        if (targetValue == null)
                        {
                            var constructor = property.PropertyType.GetConstructor(new Type[0]);
                            targetValue = (ModelBase)constructor.Invoke(new object[0]);
                        }
                        targetValue.CopyFrom(sourceValue);
                    }
                }
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(ICollection<>)))
                {
                    var interfaceType = property.PropertyType.GetInterfaces().SingleOrDefault(x => x == typeof(ICollection<>));
                    // If target type implements ICollection use target collection instead of overwriting property
                    dynamic targetCollection = property.GetValue(this);
                    IEnumerable sourceCollection = property.GetValue(sourceObject) as IEnumerable;
                    if (targetCollection == null || sourceCollection == null)
                    {
                        property.SetValue(this, sourceCollection);
                    }
                    else
                    {
                        var itemType = property.PropertyType.GetGenericArguments().First();
                        var constructor = itemType.GetConstructor(new Type[0]);
                        targetCollection.Clear();
                        foreach (dynamic item in sourceCollection)
                        {
                            if (typeof(ModelBase).IsAssignableFrom(itemType) && property.GetCustomAttribute(typeof(DeepCopyModelAttribute)) != null)
                            {
                                dynamic newItem = constructor.Invoke(new object[0]);
                                ((ModelBase)newItem).CopyFrom((ModelBase)item);
                                targetCollection.Add(newItem);
                            }
                            else
                            {
                                targetCollection.Add(item);
                            }
                        }
                    }
                }
                else
                {
                    property.SetValue(this, property.GetValue(sourceObject));
                }
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
