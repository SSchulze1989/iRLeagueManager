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
using System.Security.Cryptography.X509Certificates;

namespace iRLeagueManager.ViewModels.Collections
{
    public class ObservableModelCollection<TModel, TSource> : ReadOnlyObservableCollection<TModel>, IDisposable where TModel : class, IContainerModelBase<TSource>, new() where TSource : class, INotifyPropertyChanged
    {
        private bool NotifyCollectionActv { get; set; }

        private ObservableCollection<TModel> TargetCollection { get; }

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

        public ObservableModelCollection(bool updateItemSources = true) : base(new ObservableCollection<TModel>())
        {
            NotifyCollectionActv = true;
            TargetCollection = Items as ObservableCollection<TModel>;
            _collectionSource = new TSource[0];
            AutoUpdateItemsSources = updateItemSources;
        }

        /// <summary>
        /// Create instance of ObservableModelCollection
        /// </summary>
        /// <param name="collection">Source collection from which Model Collection is filled with data</param>
        /// <param name="updateItemSources">If true, sources of Items in the collection get automatically updated when calling UpdateSource<> Method</param>
        public ObservableModelCollection(IEnumerable<TSource> collection, bool updateItemSources = true) : this(updateItemSources)
        {
            //_collectionSource = collection != null ? collection : new TSource[0];
            NotifyCollectionActv = true;
            UpdateSource(collection);
            AutoUpdateItemsSources = updateItemSources;
            if (collection.Count() > 0)
                UpdateCollection();
        }

        public ObservableModelCollection(Action<TModel> constructorAction, bool updateItemSources = true) : this(updateItemSources)
        {
            NotifyCollectionActv = true;
            _constructorAction = constructorAction;
            //_collectionSource = new TSource[0];
            UpdateSource(new TSource[0]);
            AutoUpdateItemsSources = updateItemSources;
        }

        public ObservableModelCollection(IEnumerable<TSource> collection, Action<TModel> constructorAction, bool updateItemSources = true) : this(updateItemSources)
        {
            NotifyCollectionActv = true;
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

        //protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    if (NotifyCollectionActv)
        //        base.OnCollectionChanged(e);
        //}

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

            for (int i = 0; i < CollectionSource.Count(); i++)
            {
                var srcItem = CollectionSource.ElementAt(i);
                var trgItem = (i < TargetCollection.Count()) ? TargetCollection.ElementAt(i) : null;

                if (trgItem == null || comparer.Equals(srcItem, trgItem.GetSource()) == false)
                {
                    var findTrgItem = TargetCollection.Select((item, index) => new { item, index }).SingleOrDefault(x => comparer.Equals(srcItem, x.item.GetSource()));
                    if (findTrgItem == null)
                    {
                        trgItem = new TModel();
                        trgItem.UpdateSource(srcItem);
                        _constructorAction?.Invoke(trgItem);
                        TargetCollection.Insert(i, trgItem);
                    }
                    else
                    {
                        trgItem = findTrgItem.item;
                        TargetCollection.Move(findTrgItem.index, i);
                    }
                }

                if (srcItem != trgItem.GetSource())
                {
                    trgItem.UpdateSource(srcItem);
                }
            }

            var removeTrgItem = TargetCollection.Skip(CollectionSource.Count());
            foreach (var item in removeTrgItem.ToList())
            {
                TargetCollection.Remove(item);
            }


            //IEnumerable<TSource> except = Items.Select(x => x.GetSource()).Except(_collectionSource, comparer);
            //IEnumerable<TModel> notInCollection = Items.Where(m => except.Contains(m.GetSource())).ToList();
            //IEnumerable<TSource> notInItems = _collectionSource.Except(Items.Select(x => x.GetSource()), comparer).Where(x => x != null).ToList();

            //foreach (TModel item in notInCollection)
            //{
            //    item.Dispose();
            //    TargetCollection.Remove(item);
            //}

            //foreach (TSource item in notInItems)
            //{
            //    TModel newItem;
            //    if (item is TModel)
            //    {
            //        newItem = item as TModel;
            //    }
            //    else
            //    {
            //        newItem = new TModel();
            //        newItem.UpdateSource(item);
            //        _constructorAction?.Invoke(newItem);
            //    }
            //    TargetCollection.Add(newItem);
            //    //OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            //}

            //Sort();
        }

        public void Sort()
        {
            var sourceList = CollectionSource.ToList();
            var compareList = CollectionSource.Zip(TargetCollection, (srcItem, trgItem) => new { srcItem, trgItem });

            var changeList = new List<TModel>().Select(x => new { srcIndex = 0, trgItem = x }).ToList();

            // Check for differences
            foreach (var compare in compareList)
            {
                if (compare.srcItem != null && compare.srcItem != compare.trgItem.GetSource())
                {
                    var srcIndex = sourceList.IndexOf(compare.srcItem);
                    var trgItem = TargetCollection.SingleOrDefault(x => x.GetSource() == compare.srcItem);

                    changeList.Add(new { srcIndex, trgItem });
                }
            }

            foreach(var change in changeList)
            {
                var trgIndex = TargetCollection.IndexOf(change.trgItem);
                TargetCollection.Move(trgIndex, change.srcIndex);
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
                for (int i=0; i<Items.Count(); i++)
                {
                    var item = TargetCollection.ElementAt(i);
                    item.Dispose();
                    //TargetCollection.Remove(item);
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
