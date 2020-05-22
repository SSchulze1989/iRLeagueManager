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
using iRLeagueManager.ViewModels;

namespace iRLeagueManager.ViewModels.Collections
{
    public class ObservableModelCollection<TModel, TSource> : ObservableCollection<TModel>, IDisposable where TModel : ContainerModelBase<TSource>, new() where TSource : class, INotifyPropertyChanged
    {
        private bool NotifyCollectionActv { get; set; }

        private IEnumerable<TSource> _collectionSource;
        private IEnumerable<TSource> CollectionSource
        {
            get => _collectionSource;
            set
            {
                if (_collectionSource != null && _collectionSource is INotifyCollectionChanged oldCollection)
                {
                    oldCollection.CollectionChanged -= OnSourceCollectionChanged;
                    NotifyCollectionActv = false;
                }
                _collectionSource = value;
                OnPropertyChanged();
                if (_collectionSource != null && _collectionSource is INotifyCollectionChanged newCollection)
                {
                    newCollection.CollectionChanged += OnSourceCollectionChanged;
                    NotifyCollectionActv = true;
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
            NotifyCollectionActv = false;
            UpdateSource(collection);
            AutoUpdateItemsSources = updateItemSources;
            if (collection.Count() > 0)
                UpdateCollection();
        }

        public ObservableModelCollection(Action<TModel> constructorAction, bool updateItemSources = true) : base()
        {
            NotifyCollectionActv = false;
            _constructorAction = constructorAction;
            //_collectionSource = new TSource[0];
            UpdateSource(new TSource[0]);
            AutoUpdateItemsSources = updateItemSources;
        }

        public ObservableModelCollection(IEnumerable<TSource> collection, Action<TModel> constructorAction, bool updateItemSources = true) : base()
        {
            NotifyCollectionActv = false;
            _constructorAction = constructorAction;
            //_collectionSource = collection != null ? collection : new TSource[0];
            UpdateSource(collection);
            AutoUpdateItemsSources = updateItemSources;
            if (collection != null && collection.Count() > 0)
                UpdateCollection();
        }

        ~ObservableModelCollection() {
            NotifyCollectionActv = false;
            Dispose(false);
        }

        public void UpdateSource(IEnumerable<TSource> collection)
        {
            if (collection != null)
            {
                OnUpdateSource(_collectionSource, collection);
                CollectionSource = collection;
                if (collection.Count() > 0 || this.Count() > 0)
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
            if (NotifyCollectionActv && !disposedValue)
                UpdateCollection();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateCollection()
        {
            if (disposedValue)
                return;

            IEnumerable<TSource> except = Items.Select(x => x.GetSource()).Except(_collectionSource, comparer);
            IEnumerable<TModel> notInCollection = Items.Where(m => except.Contains(m.GetSource())).ToList();
            IEnumerable<TSource> notInItems = _collectionSource.Except(Items.Select(x => x.GetSource()), comparer).ToList();

            foreach (TModel item in notInCollection)
            {
                item.Dispose();
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

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: verwalteten Zustand (verwaltete Objekte) entsorgen.
                }

                NotifyCollectionActv = false;
                foreach (var item in Items)
                {
                    item.Dispose();
                }

                disposedValue = true;
            }
        }

        // TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
        // ~ObservableModelCollection() {
        //   // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
        //   Dispose(false);
        // }

        // Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
            Dispose(true);
            // TODO: Auskommentierung der folgenden Zeile aufheben, wenn der Finalizer weiter oben überschrieben wird.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
