using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using iRLeagueManager.Interfaces;

namespace iRLeagueManager.ViewModels
{
    public class ObservableModelCollection<TModel, TSource> : ObservableCollection<TModel> where TModel : ContainerModelBase<TSource>, new() where TSource : INotifyPropertyChanged
    {
        private IEnumerable<TSource> _collectionSource;
        private IEnumerable<TSource> CollectionSource
        {
            get => _collectionSource;
            set
            {
                if (_collectionSource != null && _collectionSource is INotifyCollectionChanged oldCollection)
                {
                    oldCollection.CollectionChanged -= OnSourceCollectionChanged;
                }
                _collectionSource = value;
                OnPropertyChanged();
                if (_collectionSource != null && _collectionSource is INotifyCollectionChanged newCollection)
                {
                    newCollection.CollectionChanged += OnSourceCollectionChanged;
                }
            }
        }
        private Action<TModel> _constructorAction;

        private readonly bool AutoUpdateItemsSources;

        public Type ModelType => typeof(TModel);

        private ContainerModelEqualityComparer<TSource> comparer = new ContainerModelEqualityComparer<TSource>();

        public ObservableModelCollection(bool updateItemSources = true) : base()
        {
            _collectionSource = new TSource[0];
            AutoUpdateItemsSources = updateItemSources;
        }

        /// <summary>
        /// Create instance of ObservableModelCollection
        /// </summary>
        /// <param name="collection">Source collection from which Model Collection is filled with data</param>
        /// <param name="updateItemSources">If true, sources of Items in the collection get automatically updated when calling UpdateSource<> Method</param>
        public ObservableModelCollection(IEnumerable<TSource> collection, bool updateItemSources = true) : base()
        {
            //_collectionSource = collection != null ? collection : new TSource[0];
            UpdateSource(collection);
            AutoUpdateItemsSources = updateItemSources;
            UpdateCollection();
        }

        public ObservableModelCollection(Action<TModel> constructorAction, bool updateItemSources = true) : base()
        {
            _constructorAction = constructorAction;
            //_collectionSource = new TSource[0];
            UpdateSource(new TSource[0]);
            AutoUpdateItemsSources = updateItemSources;
        }

        public ObservableModelCollection(IEnumerable<TSource> collection, Action<TModel> constructorAction, bool updateItemSources = true) : base()
        {
            _constructorAction = constructorAction;
            //_collectionSource = collection != null ? collection : new TSource[0];
            UpdateSource(collection);
            AutoUpdateItemsSources = updateItemSources;
            UpdateCollection();
        }

        public void UpdateSource(IEnumerable<TSource> collection)
        {
            if (collection != null)
            {
                OnUpdateSource(_collectionSource, collection);
                CollectionSource = collection;
                UpdateCollection();
                if (AutoUpdateItemsSources)
                {
                    foreach (TSource item in _collectionSource)
                    {
                        Items.SingleOrDefault(x => comparer.Equals(x.GetSource(), item))?.UpdateSource(item);
                    }
                }
            }
            else
            {
                CollectionSource = new TSource[0];
            }
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(null));
        }

        public IEnumerable<TSource> GetSource()
        {
            return _collectionSource;
        }

        private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateCollection();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateCollection()
        {
            IEnumerable<TSource> except = Items.Select(x => x.GetSource()).Except(_collectionSource, comparer);
            IEnumerable<TModel> notInCollection = Items.Where(m => except.Contains(m.GetSource())).ToList();
            IEnumerable<TSource> notInItems = _collectionSource.Except(Items.Select(x => x.GetSource()), comparer).ToList();

            foreach (TModel item in notInCollection)
            {
                this.Remove(item);
            }

            foreach (TSource item in notInItems)
            {
                TModel newItem;
                if (item is TModel)
                {
                    newItem = item as TModel;
                }
                else
                {
                    newItem = new TModel();
                    newItem.UpdateSource(item);
                    _constructorAction?.Invoke(newItem);
                }
                this.Add(newItem);
                //OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }
        }

        protected virtual void OnUpdateSource(IEnumerable<TSource> oldSource, IEnumerable<TSource> newSource)
        {
        }
    }
}
