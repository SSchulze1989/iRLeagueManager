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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using iRLeagueManager.Interfaces;
using iRLeagueManager.ViewModels;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Data;
using System.Globalization;
using System.Collections;
using iRLeagueDatabase.Extensions;

namespace iRLeagueManager.ViewModels.Collections
{
    public class ObservableViewModelCollection<TViewModel, TModel> : ReadOnlyObservableCollection<TViewModel>, IDisposable where TViewModel : class, IContainerModelBase<TModel>, new() where TModel : class, INotifyPropertyChanged, new()
    {
        private bool NotifyCollectionActive { get; set; }

        public ICollectionView CollectionView { get; }

        private ObservableCollection<TViewModel> TargetCollection { get; }

        private IEnumerable<TModel> _collectionSource;
        private IEnumerable<TModel> CollectionSource
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

        private Func<TModel, TViewModel> _constructUsing;

        private Action<TViewModel> _constructorAction;

        private readonly bool AutoUpdateItemsSources;

        public Type ModelType => typeof(TViewModel);

        private ContainerModelEqualityComparer<TModel> comparer = new ContainerModelEqualityComparer<TModel>();

        public bool PreserveViewModels { get; set; } = true;
        public bool AllowDuplicates { get; set; } = false;

        public ObservableViewModelCollection(bool updateItemSources = true) : base(new ObservableCollection<TViewModel>())
        {
            NotifyCollectionActive = true;
            TargetCollection = Items as ObservableCollection<TViewModel>;
            CollectionView = CollectionViewSource.GetDefaultView(TargetCollection);
            _collectionSource = new TModel[0];
            AutoUpdateItemsSources = updateItemSources;
        }

        /// <summary>
        /// Create instance of ObservableModelCollection
        /// </summary>
        /// <param name="collection">Source collection from which Model Collection is filled with data</param>
        /// <param name="updateItemSources">If true, sources of Items in the collection get automatically updated when calling UpdateSource<> Method</param>
        public ObservableViewModelCollection(IEnumerable<TModel> collection, bool updateItemSources = true) : this(updateItemSources)
        {
            //_collectionSource = collection != null ? collection : new TSource[0];
            NotifyCollectionActive = true;
            UpdateSource(collection);
            AutoUpdateItemsSources = updateItemSources;
            if (collection.Count() > 0)
                UpdateCollection();
        }

        public ObservableViewModelCollection(Action<TViewModel> constructorAction, bool updateItemSources = true) : this(updateItemSources)
        {
            NotifyCollectionActive = true;
            _constructorAction = constructorAction;
            //_collectionSource = new TSource[0];
            UpdateSource(new TModel[0]);
            AutoUpdateItemsSources = updateItemSources;
        }

        public ObservableViewModelCollection(Func<TModel, TViewModel> constructUsing, Action<TViewModel> constructorAction = null, bool updateItemsSource = true) : this (constructorAction, updateItemsSource)
        {
            _constructUsing = constructUsing;
        }

        public ObservableViewModelCollection(IEnumerable<TModel> collection, Action<TViewModel> constructorAction, bool updateItemSources = true) : this(updateItemSources)
        {
            NotifyCollectionActive = true;
            _constructorAction = constructorAction;
            //_collectionSource = collection != null ? collection : new TSource[0];
            UpdateSource(collection);
            AutoUpdateItemsSources = updateItemSources;
            if (collection != null && collection.Count() > 0)
                UpdateCollection();
        }

        ~ObservableViewModelCollection() {
            NotifyCollectionActive = false;
            Dispose(false);
        }

        public void UpdateSource(IEnumerable<TModel> collection)
        {
            try
            {
                lock (_collectionSource)
                {
                    if (collection != null)
                    {
                        OnUpdateSource(_collectionSource, collection);
                        CollectionSource = collection;
                        lock (Items)
                        {
                            if (collection.Count() > 0 || this.Count() > 0)
                                UpdateCollection();
                            if (AutoUpdateItemsSources)
                            {
                                foreach (TModel item in _collectionSource)
                                {
                                    Items
                                        .Where(x => comparer.Equals(x.GetSource(), item))
                                        .ForEach(x => x.UpdateSource(item ?? new TModel()));
                                }
                            }
                        }
                    }
                    else
                    {
                        CollectionSource = new TModel[0];
                    }
                }
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(null));
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
                throw;
            }
        }

        //protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    if (NotifyCollectionActv)
        //        base.OnCollectionChanged(e);
        //}

        public IEnumerable<TModel> GetSource()
        {
            return _collectionSource;
        }

        private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (NotifyCollectionActive && !disposedValue)
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

            if (PreserveViewModels && AllowDuplicates)
            {
                throw new InvalidOperationException($"{nameof(PreserveViewModels)} and {nameof(AllowDuplicates)} cannot both be set true at the same time");
            }

            lock (_collectionSource)
            lock (TargetCollection)
            {
                    IEnumerable<TModel> localSource;
                    if (AllowDuplicates == false)
                    {
                        localSource = _collectionSource.Distinct(comparer);
                    }
                    else
                    {
                        localSource = _collectionSource;
                    }
                    try
                    {
                        if (PreserveViewModels)
                        {
                            for (int i = 0; i < localSource.Count(); i++)
                            {
                                var srcItem = localSource.ElementAt(i) ?? new TModel();
                                var trgItem = (i < TargetCollection.Count()) ? TargetCollection.ElementAt(i) : null;

                                if (trgItem == null || comparer.Equals(srcItem, trgItem.GetSource()) == false)
                                {
                                    var findTrgItem = TargetCollection.Select((item, index) => new { item, index }).SingleOrDefault(x => comparer.Equals(srcItem, x.item.GetSource()));
                                    if (findTrgItem == null)
                                    {
                                        trgItem = ConstructViewModel(srcItem);
                                        if (i < TargetCollection.Count())
                                        {
                                            TargetCollection.Insert(i, trgItem);
                                        }
                                        else
                                        {
                                            TargetCollection.Add(trgItem);
                                        }
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

                            var removeTrgItem = TargetCollection.Skip(_collectionSource.Count());
                            foreach (var item in removeTrgItem.ToList())
                            {
                                TargetCollection.Remove(item);
                            }
                        }
                        else
                        {
                            TargetCollection.Clear();
                            foreach (var srcItem in localSource)
                            {
                                var trgItem = ConstructViewModel(srcItem);
                                TargetCollection.Add(trgItem);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        GlobalSettings.LogError(e);
                        throw;
                    }
            }

            CollectionView?.Refresh();
        }

        private TViewModel ConstructViewModel(TModel srcItem)
        {
            TViewModel trgItem;
            if (_constructUsing == null)
                trgItem = new TViewModel();
            else
                trgItem = _constructUsing.Invoke(srcItem);

            trgItem.UpdateSource(srcItem);
            _constructorAction?.Invoke(trgItem);
            return trgItem;
        }

        public void Sort()
        {
            var sourceList = CollectionSource.ToList();
            var compareList = CollectionSource.Zip(TargetCollection, (srcItem, trgItem) => new { srcItem, trgItem });

            var changeList = new List<TViewModel>().Select(x => new { srcIndex = 0, trgItem = x }).ToList();

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

        protected virtual void OnUpdateSource(IEnumerable<TModel> oldSource, IEnumerable<TModel> newSource)
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

                NotifyCollectionActive = false;
                //for (int i=0; i<Items.Count(); i++)
                //{
                //    var item = TargetCollection.ElementAt(i);
                //    //item.Dispose();
                //    TargetCollection.Remove(item);
                //}
                _collectionSource = new TModel[0];

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
