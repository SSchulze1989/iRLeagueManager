using iRLeagueDatabase.Filters;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Filters;
using iRLeagueManager.Models.Results;
using iRLeagueManager.ViewModels.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace iRLeagueManager.ViewModels
{
    public class FilterEditViewModel : ViewModelBase
    {
        private ScoringModel Scoring { get; set; }

        private ObservableCollection<ResultsFilterOptionModel> filterOptionsSource;
        private ObservableCollection<ResultsFilterOptionModel> FilterOptionsSource
        {
            get => filterOptionsSource;
            set
            {
                if (SetValue(ref filterOptionsSource, value))
                {
                    resultsFilterOptions.UpdateSource(FilterOptionsSource);
                }
            }
        }
        private readonly ObservableModelCollection<ResultsFilterOptionViewModel, ResultsFilterOptionModel> resultsFilterOptions;
        public ICollectionView ResultsFilterOptions => resultsFilterOptions.CollectionView;
        public IEnumerable<string> FilterTypes { get; }
        public IEnumerable<string> FilterProperties { get; }

        public ICommand RemoveFilterCmd { get; }
        public ICommand AddFilterCmd { get; }

        public event ActionDialogEventHandler<FilterEditViewModel> ViewOpenActionDialog;

        private List<ResultsFilterOptionModel> addFilters { get; } = new List<ResultsFilterOptionModel>();
        private List<ResultsFilterOptionModel> removeFilters { get; } = new List<ResultsFilterOptionModel>();

        public FilterEditViewModel()
        {
            List<string> excludeProperties = typeof(MappableModel).GetProperties().Select(x => x.Name).ToList();
            excludeProperties.Add(nameof(ResultRowModel.ResultId));
            excludeProperties.Add(nameof(ResultRowModel.ResultRowId));
            excludeProperties.Add(nameof(ResultRowModel.SimSessionType));
            excludeProperties.Add(nameof(ResultRowModel.Location));
            FilterProperties = typeof(ResultRowModel).GetProperties().Select(x => x.Name).Except(excludeProperties);

            resultsFilterOptions = new ObservableModelCollection<ResultsFilterOptionViewModel, ResultsFilterOptionModel>();
            var filters = new List<ResultsFilterOptionModel>()
            {
                new ResultsFilterOptionModel()
                {
                    ResultsFilterType = "ColumnPropertyFilter",
                    Comparator = Enums.ComparatorTypeEnum.IsEqual,
                    ColumnPropertyName = FilterProperties.First(),
                    FilterValues = new System.Collections.ObjectModel.ObservableCollection<object>(new string[] { "Test", "Test2"} )
                }
            };
            //resultsFilterOptions.UpdateSource(filters);
            FilterOptionsSource = new ObservableCollection<ResultsFilterOptionModel>(filters);

            AddFilterCmd = new RelayCommand(o => AddFilter(), o => Scoring != null);
            RemoveFilterCmd = new RelayCommand(async o =>
            {
                if (ViewOpenActionDialog != null)
                {
                    ViewOpenActionDialog.Invoke(this, "Delete Filter", "Really delete Filter?", x => x.RemoveFilter(((ResultsFilterOptionViewModel)o).Model));
                }
                else
                {
                    RemoveFilter(((ResultsFilterOptionViewModel)o).Model);
                }
            }, o => o != null && o is ResultsFilterOptionViewModel);
        }

        public async Task Load(ScoringModel scoring)
        {
            Scoring = scoring;
            if (scoring == null)
            {
                resultsFilterOptions.UpdateSource(null);
                return;
            }

            try
            {
                IsLoading = true;
                var filters = await LeagueContext.GetModelsAsync<ResultsFilterOptionModel>(Scoring.ResultsFilterOptionIds);
                FilterOptionsSource = new ObservableCollection<ResultsFilterOptionModel>(filters);
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

        public void AddFilter()
        {
            if (Scoring == null)
            {
                return;
            }

            try
            {
                IsLoading = true;
                var newFilter = new ResultsFilterOptionModel(-FilterOptionsSource.Count,  Scoring.ScoringId.GetValueOrDefault())
                {
                    ResultsFilterType = "ColumnPropertyFilter",
                    ColumnPropertyName = FilterProperties.First(),
                    Comparator = Enums.ComparatorTypeEnum.IsEqual,
                    FilterValues = new ObservableCollection<object>()
                };
                //await LeagueContext.AddModelAsync(newFilter);
                addFilters.Add(newFilter);
                FilterOptionsSource.Add(newFilter);
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

        public void RemoveFilter(ResultsFilterOptionModel filter)
        {
            try
            {
                IsLoading = true;
                //await LeagueContext.DeleteModelAsync<ResultsFilterOptionModel>(filter.ModelId);
                if (addFilters.Contains(filter))
                {
                    addFilters.Remove(filter);
                }
                else if (removeFilters.Contains(filter) == false)
                {
                    removeFilters.Add(filter);
                }

                if (FilterOptionsSource.Contains(filter))
                {
                    FilterOptionsSource.Remove(filter);
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

        public async Task<bool> SaveChanges()
        {
            if (Scoring == null)
            {
                return true;
            }

            try
            {
                IsLoading = true;
                List<Task> taskList = new List<Task>();
                taskList.Add(LeagueContext.AddModelsAsync(addFilters.ToArray()));
                taskList.Add(LeagueContext.DeleteModelsAsync(removeFilters.ToArray()));
                taskList.Add(LeagueContext.UpdateModelsAsync(FilterOptionsSource.Where(x => x.ContainsChanges)));
                await Task.WhenAll(taskList.ToArray());
                await LeagueContext.UpdateModelAsync(Scoring);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
                return false;
            }
            finally
            {
                IsLoading = false;
            }
            return true;
        }
    }
}
