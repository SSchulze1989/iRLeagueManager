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
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.ViewModels.Collections;
using iRLeagueManager.Models.Reviews;
using System.Collections.ObjectModel;
using iRLeagueManager.Models.Statistics;
using System.Windows;
using System.Collections.Specialized;

namespace iRLeagueManager.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase, ISeasonPageViewModel
    {
        private SeasonViewModel season;
        public SeasonViewModel Season
        {
            get => season;
            set
            {
                var tmp = season;
                if (SetValue(ref season, value))
                {
                    if (tmp != null)
                    {
                        tmp.PropertyChanged -= Season_PropertyChanged;
                        tmp.ModelChanged -= Season_ModelChanged;
                    }
                    if (season != null)
                    {
                        season.PropertyChanged += Season_PropertyChanged;
                        season.ModelChanged += Season_ModelChanged;
                    }
                }
            }
        }

        private readonly ObservableViewModelCollection<ScoringViewModel, ScoringModel> scorings;
        //public ObservableModelCollection<ScoringViewModel, ScoringModel> Scorings
        //{
        //    get
        //    {
        //        if (scorings.GetSource() != Season?.Scorings)
        //            scorings.UpdateSource(Season?.Scorings);
        //        return scorings;
        //    }
        //}
        public ICollectionView Scorings
        {
            get
            {
                if (scorings.GetSource() != Season?.Scorings)
                    scorings.UpdateSource(Season?.Scorings);
                return scorings.CollectionView;
            }
        }

        private readonly ObservableViewModelCollection<ScoringTableViewModel, ScoringTableModel> scoringTables;
        //public ObservableModelCollection<ScoringTableViewModel, ScoringTableModel> ScoringTables
        //{
        //    get
        //    {
        //        if (scoringTables.GetSource() != Season?.ScoringTables)
        //            scoringTables.UpdateSource(Season?.ScoringTables);
        //        return scoringTables;
        //    }
        //}
        public ICollectionView ScoringTables
        {
            get
            {
                if (scoringTables.GetSource() != Season?.ScoringTables)
                    scoringTables.UpdateSource(Season?.ScoringTables);
                return scoringTables.CollectionView;
            }
        }

        //private readonly ObservableViewModelCollection<StatisticSetViewModel, StatisticSetModel> seasonStatistics;
        //public ICollectionView SeasonStatistics => seasonStatistics.CollectionView;

        //private readonly ObservableViewModelCollection<StatisticSetViewModel, StatisticSetModel> leagueStatistics;
        //public ICollectionView LeagueStatistics => leagueStatistics.CollectionView;

        //private StatisticSetViewModel selectedStatisticSet;
        //public StatisticSetViewModel SelectedStatisticSet { get => selectedStatisticSet; set => SetValue(ref selectedStatisticSet, value); }

        private readonly ObservableViewModelCollection<StatisticSetViewModel, StatisticSetModel> statisticSets;
        public ICollectionView StatisticSets => statisticSets.CollectionView;

        private ObservableCollection<VoteCategoryModel> voteCategoriesCollection;
        private ICollectionView voteCategories;
        public ICollectionView VoteCategories { get => voteCategories; set => SetValue(ref voteCategories, value); }

        private ObservableCollection<CustomIncidentModel> incidentKindsCollection;
        private ICollectionView incidentKinds;
        public ICollectionView IncidentKinds { get => incidentKinds; set => SetValue(ref incidentKinds, value); }

        public ICommand AddScoringCmd { get; }
        public ICommand DeleteScoringCmd { get; }
        public ICommand AddScoringTableCmd { get; }
        public ICommand DeleteScoringTableCmd { get; }

        public ICommand AddIncidentKindCmd { get; }
        public ICommand RemoveIncidentKindCmd { get; }
        public ICommand AddVoteCategoryCmd { get; }
        public ICommand RemoveVoteCategoryCmd { get; }

        public ICommand AddSeasonStatisticSetCmd { get; }
        public ICommand AddLeagueStatisticSetCmd { get; }
        public ICommand AddImportedStatisticSetCmd { get; }

        public ICommand SaveChangesCmd { get; }

        public SettingsPageViewModel() : base()
        {
            scorings = new ObservableViewModelCollection<ScoringViewModel, ScoringModel>(constructorAction: x => x.SetScoringsList(scorings.GetSource()));
            scoringTables = new ObservableViewModelCollection<ScoringTableViewModel, ScoringTableModel>(x => x.SetScoringsList(scorings));
            Season = new SeasonViewModel();
            AddScoringCmd = new RelayCommand(o => AddScoring(), o => Season != null);
            AddScoringTableCmd = new RelayCommand(o => AddScoringTable(), o => Season != null);
            DeleteScoringCmd = new RelayCommand(o => DeleteScoring((o as ScoringViewModel).Model), o => o != null);
            DeleteScoringTableCmd = new RelayCommand(o => DeleteScoringTable((o as ScoringTableViewModel).Model), o => o != null);
            SaveChangesCmd = new RelayCommand(async o => await SaveChanges(), o => CanSaveChanges());
            AddIncidentKindCmd = new RelayCommand(async o => await AddIncidentKind());
            RemoveIncidentKindCmd = new RelayCommand(async o => await RemoveIncidentKind(o as CustomIncidentModel), o => o != null && o is CustomIncidentModel);
            AddVoteCategoryCmd = new RelayCommand(async o => await AddVoteCategory());
            RemoveVoteCategoryCmd = new RelayCommand(async o => await RemoveVoteCategory(o as VoteCategoryModel), o => o != null && o is VoteCategoryModel);
            //seasonStatistics = new ObservableViewModelCollection<StatisticSetViewModel, StatisticSetModel>(x => GetStatisticSetViewModel(x));
            //leagueStatistics = new ObservableViewModelCollection<StatisticSetViewModel, StatisticSetModel>(x => GetStatisticSetViewModel(x));
            statisticSets = new ObservableViewModelCollection<StatisticSetViewModel, StatisticSetModel>(x => GetStatisticSetViewModel(x));
            StatisticSets.GroupDescriptions.Add(new PropertyGroupDescription("StatisticSetType"));
            AddSeasonStatisticSetCmd = new RelayCommand(async o => await AddStatisticSet(new SeasonStatisticSetModel(Season.Model)), o => Season.Model != null);
            AddLeagueStatisticSetCmd = new RelayCommand(async o => await AddStatisticSet(new LeagueStatisticSetModel()), o => true);
            AddImportedStatisticSetCmd = new RelayCommand(async o => await AddStatisticSet(new ImportedStatisticSetModel()), o => true);
        }

        private StatisticSetViewModel GetStatisticSetViewModel(StatisticSetModel model)
        {
            var type = model.GetType();

            if (type.Equals(typeof(SeasonStatisticSetModel)))
            {
                return new SeasonStatisticSetViewModel();
            }
            else if (type.Equals(typeof(LeagueStatisticSetModel)))
            {
                return new LeagueStatisticSetViewModel();
            } else if (type.Equals(typeof(ImportedStatisticSetModel)))
            {
                return new ImportedStatisticSetViewModel();
            }

            return new StatisticSetViewModel();
        }

        private void Season_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SeasonModel.SeasonStatisticSets):
                    break;
                default:
                    break;
            }
        }

        private void Season_ModelChanged()
        {
            
        }

        public async Task Load(SeasonModel season)
        {
            try
            {
                IsLoading = true;
                if (season == null)
                {
                    Season = null;
                    StatusMsg = "No season loaded";
                    OnPropertyChanged(null);
                    return;
                }

                //Season = new SeasonViewModel(season);
                Season.UpdateSource(season);
                OnPropertyChanged(null);

                await LeagueContext.GetModelAsync<SeasonModel>(season.ModelId);
                var incidentKinds = await LeagueContext.GetModelsAsync<CustomIncidentModel>();
                incidentKindsCollection = new ObservableCollection<CustomIncidentModel>(incidentKinds);
                SetIncidentKindsView(incidentKindsCollection);
                var voteCategories = await LeagueContext.GetModelsAsync<VoteCategoryModel>();
                voteCategoriesCollection = new ObservableCollection<VoteCategoryModel>(voteCategories);
                SetVoteCategoriesView(voteCategoriesCollection);
                await LoadStatisticSets();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SetVoteCategoriesView(object source)
        {
            VoteCategories = CollectionViewSource.GetDefaultView(source);
            VoteCategories.SortDescriptions.Add(new SortDescription(nameof(VoteCategoryModel.Index), ListSortDirection.Ascending));
        }

        private void SetIncidentKindsView(object source)
        {
            IncidentKinds = CollectionViewSource.GetDefaultView(source);
            IncidentKinds.SortDescriptions.Add(new SortDescription(nameof(CustomIncidentModel.Index), ListSortDirection.Ascending));
        }

        private async Task AddStatisticSet(StatisticSetModel statisticSet)
        {
            if (statisticSet == null)
            {
                return;
            }

            try
            {
                IsLoading = true;
                if (statisticSet is SeasonStatisticSetModel seasonStatisticSet)
                {
                    statisticSet = await LeagueContext.AddModelAsync(seasonStatisticSet);
                }
                else if (statisticSet is LeagueStatisticSetModel leagueStatisticSet)
                {
                    statisticSet = await LeagueContext.AddModelAsync(leagueStatisticSet);
                }
                else if (statisticSet is ImportedStatisticSetModel importedStatisticSet)
                {
                    statisticSet = await LeagueContext.AddModelAsync(importedStatisticSet);
                }
                await LoadStatisticSets();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddIncidentKind()
        {
            try
            {
                IsLoading = true;
                var newIncident = await LeagueContext.AddModelAsync(new CustomIncidentModel() { Index = incidentKindsCollection.Count > 0 ? incidentKindsCollection.Max(x => x.Index) + 1 : 0 });
                incidentKindsCollection.Add(newIncident);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task RemoveIncidentKind(CustomIncidentModel incidentKind)
        {
            if (incidentKind == null)
            {
                return;
            }

            try
            {
                IsLoading = true;
                await LeagueContext.DeleteModelAsync<CustomIncidentModel>(incidentKind.ModelId);
                incidentKindsCollection.Remove(incidentKind);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddVoteCategory()
        {
            try
            {
                IsLoading = true;
                var newCategory = await LeagueContext.AddModelAsync(new VoteCategoryModel() { Index = voteCategoriesCollection.Count > 0 ? voteCategoriesCollection.Max(x => x.Index) + 1 : 0 });
                voteCategoriesCollection.Add(newCategory);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task RemoveVoteCategory(VoteCategoryModel voteCategory)
        {
            if (voteCategory == null)
            {
                return;
            }

            try
            {
                IsLoading = true;
                await LeagueContext.DeleteModelAsync<VoteCategoryModel>(voteCategory.ModelId);
                voteCategoriesCollection.Remove(voteCategory);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public override async Task Refresh()
        {
            try
            {
                IsLoading = true;
                LeagueContext.ModelManager.ForceExpireModels<SeasonModel>();
                LeagueContext.ModelManager.ForceExpireModels<ScoringModel>();
                LeagueContext.ModelManager.ForceExpireModels<ScoringTableModel>();
                LeagueContext.ModelManager.ForceExpireModels<CustomIncidentModel>();
                LeagueContext.ModelManager.ForceExpireModels<VoteCategoryModel>();
                LeagueContext.ModelManager.ForceExpireModels<StatisticSetModel>();
                await Load(Season.Model);
                foreach(StatisticSetViewModel statisticSet in StatisticSets)
                {
                    await statisticSet.Refresh();
                }
                await base.Refresh();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadStatisticSets()
        {   
            try
            {
                IsLoading = true;
                var statisticSetsSource = (await LeagueContext.GetModelsAsync<StatisticSetModel>())
                    .Where(x => x is SeasonStatisticSetModel == false || ((SeasonStatisticSetModel)x).Season.SeasonId == Season?.SeasonId);
                var leagueStatistics = statisticSetsSource.OfType<LeagueStatisticSetModel>();
                foreach(var leagueStatistic in leagueStatistics)
                {
                    for (int i = 0; i < leagueStatistic.StatisticSets.Count; i++)
                    {
                        leagueStatistic.StopTrackChanges();
                        leagueStatistic.StatisticSets[i] = LeagueContext.ModelManager.ModelCache.PutOrGetModel(leagueStatistic.StatisticSets[i]);
                        leagueStatistic.StartTrackChanges();
                    }
                }
                //seasonStatistics.UpdateSource(seasonStatisticSets);
                //leagueStatistics.UpdateSource(statisticSets.Except(seasonStatisticSets));
                statisticSets.UpdateSource(statisticSetsSource);
                SetStatisticSetSelection();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void SetStatisticSetSelection()
        {
            var source = statisticSets.Select(x => x.Model);
            foreach (var leagueStatistic in statisticSets.OfType<LeagueStatisticSetViewModel>())
            {
                leagueStatistic.SetStatisticSetSelection(source);
            }
        }

        public async void AddScoring()
        {
            try
            {
                IsLoading = true;
                var scoring = new ScoringModel() { Season = this.Season.Model, Name = "New Scoring" };
                scoring = await LeagueContext.AddModelAsync(scoring);
                Season.Scorings.Add(scoring);
                await LeagueContext.UpdateModelAsync(Season.Model);
                scorings.UpdateCollection();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void DeleteScoring(ScoringModel scoring)
        {
            if (scoring == null)
                return;

            try
            {
                IsLoading = true;
                await LeagueContext.DeleteModelsAsync(scoring);
                Season.Scorings.Remove(scoring);
                await LeagueContext.UpdateModelAsync(Season.Model);
                scorings.UpdateCollection();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void AddScoringTable()
        {
            try
            {
                IsLoading = true;
                var scoringTable = new ScoringTableModel() { Name = "New Scoring Table" };
                //scoringTable = await LeagueContext.AddModelAsync(scoringTable);
                Season.ScoringTables.Add(scoringTable);
                await LeagueContext.UpdateModelAsync(Season.Model);
                scoringTables.UpdateCollection();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void DeleteScoringTable(ScoringTableModel scoringTable)
        {
            if (scoringTable == null)
                return;

            try
            {
                IsLoading = true;
                await LeagueContext.DeleteModelsAsync(scoringTable);
                Season.ScoringTables.Remove(scoringTable);
                await LeagueContext.UpdateModelAsync(Season.Model);
                scoringTables.UpdateCollection();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task SaveChanges()
        {
            try
            {
                IsLoading = true;
                if (Season.Model.ContainsChanges)
                {
                    await Season.SaveChanges();
                }

                foreach (var scoring in scorings)
                {
                    await scoring.SaveChanges();
                }
                foreach (var scoringTable in scoringTables)
                {
                    await scoringTable.SaveChanges();
                }
                foreach (var statisticSet in statisticSets)
                {
                    await statisticSet.SaveChanges();
                }

                var updateIncidenKinds = incidentKindsCollection.Where(x => x.ContainsChanges);
                if (updateIncidenKinds.Count() > 0)
                {
                    await LeagueContext.UpdateModelsAsync(updateIncidenKinds);
                }
                var updateVoteCategories = voteCategoriesCollection.Where(x => x.ContainsChanges);
                if (updateVoteCategories.Count() > 0)
                {
                    await LeagueContext.UpdateModelsAsync(updateVoteCategories);
                }
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public bool CanSaveChanges()
        {
            if (Season == null)
            {
                return false;
            }

            var hasChanges = Season.Model.ContainsChanges;
            foreach (var scoring in scorings)
            {
                hasChanges |= scoring.Model.ContainsChanges;
            }
            foreach (var scoringTable in scoringTables)
            {
                hasChanges |= scoringTable.Model.ContainsChanges;
            }
            hasChanges |= statisticSets.Select(x => x.Model.ContainsChanges).Any(x => x);
            if (incidentKindsCollection != null)
            {
                hasChanges |= incidentKindsCollection.Any(x => x.ContainsChanges);
            }
            if (voteCategoriesCollection != null)
            {
                hasChanges |= voteCategoriesCollection.Any(x => x.ContainsChanges);
            }

            return hasChanges;
        }

        protected override void Dispose(bool disposing)
        {
            season.Dispose();
            scorings.Dispose();
            scoringTables.Dispose();
            base.Dispose(disposing);
        }
    }
}
