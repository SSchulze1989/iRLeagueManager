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

using iRLeagueManager.Models.Statistics;
using iRLeagueManager.ViewModels.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace iRLeagueManager.ViewModels
{
    public class LeagueStatisticSetViewModel : StatisticSetViewModel, IContainerModelBase<LeagueStatisticSetModel>
    {
        public new LeagueStatisticSetModel Model => base.Model as LeagueStatisticSetModel;

        private readonly ObservableViewModelCollection<StatisticSetViewModel, StatisticSetModel> statisticSets;
        public ICollectionView StatisticSets
        {
            get
            {
                if (statisticSets.GetSource() != Model?.StatisticSets)
                {
                    statisticSets.UpdateSource(Model?.StatisticSets);
                    if (statisticSets.CollectionView.CanFilter)
                    {
                        statisticSets.CollectionView.Refresh();
                    }
                }
                return statisticSets.CollectionView;
            }
        }

        private ICollectionView statisticSetSelection;
        public ICollectionView StatisticSetSelection { get => statisticSetSelection; private set => SetValue(ref statisticSetSelection, value); }

        public ICommand AddToSelectionCmd { get; }
        public ICommand RemoveFromSelectionCmd { get; }

        protected override StatisticSetModel Template => new LeagueStatisticSetModel();

        public LeagueStatisticSetViewModel()
        {
            AddToSelectionCmd = new RelayCommand(o =>
            {
                if (o is IList selected)
                {
                    foreach (var set in selected.OfType<StatisticSetModel>().ToList())
                    {
                        AddToSelection(set);
                    }
                }
                else
                {
                    AddToSelection((StatisticSetModel)o);
                }
            }, o => o is StatisticSetModel || (o as IList)?.OfType<StatisticSetModel>().Count() > 0);
            RemoveFromSelectionCmd = new RelayCommand(o =>
            {
                if (o is IList selected)
                {
                    foreach (var set in selected.OfType<StatisticSetViewModel>().ToList())
                    {
                        RemoveFromSelection(set.Model);
                    }
                }
                else
                {
                    RemoveFromSelection(((StatisticSetViewModel)o).Model);
                }
            }, o => o is StatisticSetViewModel || (o as IList)?.OfType<StatisticSetViewModel>().Count() > 0);
            statisticSets = new ObservableViewModelCollection<StatisticSetViewModel, StatisticSetModel>(x => StatisticSetViewModel.GetStatisticSetViewModel(x.GetType()));
            var groupDescription = new PropertyGroupDescription("StatisticSetType");
            StatisticSets.GroupDescriptions.Add(groupDescription);
        }

        public void SetStatisticSetSelection(IEnumerable<StatisticSetModel> source)
        {
            StatisticSetSelection = CollectionViewSource.GetDefaultView(source);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("StatisticSetType");
            StatisticSetSelection.Filter = StatisticSetFilter;
            StatisticSetSelection.GroupDescriptions.Clear();
            StatisticSetSelection.GroupDescriptions.Add(groupDescription);
        }

        public bool StatisticSetFilter(object item)
        {
            if (item is StatisticSetModel statisticSet)
            {
                if (statisticSet.StatisticSetType == "League" || StatisticSets.SourceCollection.OfType<StatisticSetViewModel>().Any(x => x.Id == statisticSet.Id))
                {
                    return false;
                }
            }
            return true;
        }

        public void AddToSelection(StatisticSetModel model)
        {
            if (model != null && statisticSets.Any(x => x.Id == model.Id) == false)
            {
                Model.StatisticSets.Add(model);
            }
            if (StatisticSetSelection.CanFilter)
            {
                StatisticSetSelection.Refresh();
            }
        }

        public void RemoveFromSelection(StatisticSetModel model)
        {
            if (model != null && statisticSets.Any(x => x.Id == model.Id))
            {
                Model.StatisticSets.Remove(model);
            }
            if (StatisticSetSelection.CanFilter)
            {
                StatisticSetSelection.Refresh();
            }
        }

        public override Task Refresh()
        {
            if (StatisticSets.CanFilter)
            {
                StatisticSets.Refresh();
            }
            return base.Refresh();
        }

        public bool UpdateSource(LeagueStatisticSetModel source)
        {
            return base.UpdateSource(source);
        }

        LeagueStatisticSetModel IContainerModelBase<LeagueStatisticSetModel>.GetSource()
        {
            return Model;
        }
    }
}
