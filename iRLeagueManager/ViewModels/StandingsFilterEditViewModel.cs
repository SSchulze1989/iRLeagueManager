using iRLeagueDatabase.Filters;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Filters;
using iRLeagueManager.Models.Members;
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
    public class StandingsFilterEditViewModel : ViewModelBase
    {
        private ScoringTableModel ScoringTable { get; set; }

        private ObservableCollection<StandingsFilterOptionModel> filterOptionsSource;
        private ObservableCollection<StandingsFilterOptionModel> FilterOptionsSource
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
        private readonly ObservableViewModelCollection<StandingsFilterOptionViewModel, StandingsFilterOptionModel> resultsFilterOptions;
        public ICollectionView ResultsFilterOptions => resultsFilterOptions.CollectionView;
        public IEnumerable<string> FilterTypes { get; }
        public IEnumerable<string> FilterProperties { get; }

        public ICommand RemoveFilterCmd { get; }
        public ICommand AddFilterCmd { get; }

        public event ActionDialogEventHandler<StandingsFilterEditViewModel> ViewOpenActionDialog;

        private List<StandingsFilterOptionModel> addFilters { get; } = new List<StandingsFilterOptionModel>();
        private List<StandingsFilterOptionModel> removeFilters { get; } = new List<StandingsFilterOptionModel>();

        public static MemberListViewModel MemberList => new MemberListViewModel();

        public StandingsFilterEditViewModel()
        {
            List<string> excludeProperties = typeof(MappableModel).GetProperties().Select(x => x.Name).ToList();
            excludeProperties.Add(nameof(ResultRowModel.ResultId));
            excludeProperties.Add(nameof(ResultRowModel.ResultRowId));
            excludeProperties.Add(nameof(ResultRowModel.SimSessionType));
            excludeProperties.Add(nameof(ResultRowModel.Location));
            excludeProperties.Add(nameof(ResultRowModel.MemberId));
            FilterProperties = typeof(ResultRowModel)
                .GetProperties()
                .Select(x => x.Name)
                .Except(excludeProperties);

            resultsFilterOptions = new ObservableViewModelCollection<StandingsFilterOptionViewModel, StandingsFilterOptionModel>();
            var filters = new List<StandingsFilterOptionModel>()
            {
                new StandingsFilterOptionModel()
                {
                    FilterType = "ColumnPropertyFilter",
                    Comparator = Enums.ComparatorTypeEnum.IsEqual,
                    ColumnPropertyName = FilterProperties.First(),
                }
            };
            filters.First().FilterValues = new ObservableCollection<FilterValueModel>(new FilterValueModel[]
            {
                new FilterValueModel() { ValueType = filters.First().ColumnPropertyType, Value = "Test1" },
                new FilterValueModel() { ValueType = filters.First().ColumnPropertyType, Value = "Test2" }
            });

            //resultsFilterOptions.UpdateSource(filters);
            FilterOptionsSource = new ObservableCollection<StandingsFilterOptionModel>(filters);

            AddFilterCmd = new RelayCommand(o => AddFilter(), o => ScoringTable != null);
            RemoveFilterCmd = new RelayCommand(o =>
            {
                if (ViewOpenActionDialog != null)
                {
                    ViewOpenActionDialog.Invoke(this, "Delete Filter", "Really delete Filter?", x => x.RemoveFilter(((StandingsFilterOptionViewModel)o).Model));
                }
                else
                {
                    RemoveFilter(((StandingsFilterOptionViewModel)o).Model);
                }
            }, o => o != null && o is StandingsFilterOptionViewModel);
        }

        public async Task Load(ScoringTableModel scoring)
        {
            ScoringTable = scoring;
            if (scoring == null)
            {
                resultsFilterOptions.UpdateSource(null);
                return;
            }

            try
            {
                IsLoading = true;
                var filters = await LeagueContext.GetModelsAsync<StandingsFilterOptionModel>(ScoringTable.StandingsFilterOptionIds);
                FilterOptionsSource = new ObservableCollection<StandingsFilterOptionModel>(filters);
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
            LeagueContext.ModelManager.ForceExpireModels<StandingsFilterOptionModel>(ScoringTable.StandingsFilterOptionIds.Select(x => new long[] { x }));
            await Load(ScoringTable);
        }

        public void AddFilter()
        {
            if (ScoringTable == null)
            {
                return;
            }

            try
            {
                IsLoading = true;
                var newFilter = new StandingsFilterOptionModel(-FilterOptionsSource.Count,  ScoringTable.ScoringTableId)
                {
                    FilterType = "ColumnPropertyFilter",
                    ColumnPropertyName = FilterProperties.First(),
                    Comparator = Enums.ComparatorTypeEnum.IsEqual,
                    FilterValues = new ObservableCollection<FilterValueModel>()
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

        public void RemoveFilter(StandingsFilterOptionModel filter)
        {
            try
            {
                IsLoading = true;
                //await LeagueContext.DeleteModelAsync<StandingsFilterOptionModel>(filter.ModelId);
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
            if (ScoringTable == null)
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
                await LeagueContext.UpdateModelAsync(ScoringTable);
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
