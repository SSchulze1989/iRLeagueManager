using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

using iRLeagueManager;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Data;
using System.Collections;

namespace iRLeagueManager.ViewModels
{
    public class SchedulerViewModel : ViewModelBase, INotifyPropertyChanged, INotifyCollectionChanged, IEnumerable<ScheduleViewModel>
    {
        private LeagueContext LeagueContext => GlobalSettings.LeagueContext;

        private ObservableModelCollection<ScheduleViewModel, ScheduleModel> scheduleList;
        private ObservableModelCollection<ScheduleViewModel, ScheduleModel> ScheduleList
        {
            get => scheduleList;
            set
            {
                if (SetValue(ref scheduleList, value, (t, v) => t.GetSource().Equals(v.GetSource())))
                {
                    OnPropertyChanged(null);
                }
            }
        }

        private SeasonModel season;
        public SeasonModel Season { get => season; set => SetValue(ref season, value); }

        public  ICommand CreateScheduleCmd { get; }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                ((INotifyCollectionChanged)ScheduleList).CollectionChanged += value;
            }

            remove
            {
                ((INotifyCollectionChanged)ScheduleList).CollectionChanged -= value;
            }
        }

        public SchedulerViewModel()
        {
            ScheduleList = new ObservableModelCollection<ScheduleViewModel, ScheduleModel>(new ScheduleModel[] { ScheduleModel.GetTemplate() });
            CreateScheduleCmd = new RelayCommand(o => CreateSchedule(), o => Season != null);
        }

        public async void CreateSchedule()
        {
            var newSchedule = new ScheduleModel()
            {
                Name = "New Schedule"
            };

            Season.Schedules.Add(newSchedule);
            await LeagueContext.UpdateModelAsync(Season);
            //Load(Season);
        }

        public async void Load(SeasonModel season)
        {
            var loadedModels = new List<ScheduleModel>();
            Season = season;
            if (season == null || season.Schedules.Count == 0)
            {
                ScheduleList.UpdateSource(new ScheduleModel[] { ScheduleModel.GetTemplate() });
                return;
            }
            else
            {
                loadedModels = ScheduleList.Select(x => x.GetSource()).Where(x => season.Schedules.Select(y => y.ScheduleId).Contains(x.ScheduleId)).ToList();
            }

            var newIds = season.Schedules.Select(x => x.ScheduleId.Value).Except(loadedModels.Select(x => x.ScheduleId.Value)).ToList();

            List<ScheduleModel> schedules = new List<ScheduleModel>();

            if (loadedModels.Count() > 0)
            {
                schedules.AddRange(await LeagueContext.UpdateModelsAsync(loadedModels));
            }
            if (newIds.Count() > 0)
            {
                schedules.AddRange(await LeagueContext.GetModelsAsync<ScheduleModel>(newIds));
            }

            //var scheduleIds = season.Schedules.Select(x => x.ScheduleId.Value);
            //schedules = await LeagueContext.GetModelsAsync<ScheduleModel>(scheduleIds);
            schedules = schedules.OrderBy(x => x.ScheduleId).ToList();

            ScheduleList.UpdateSource(schedules);
        }

        public async void DeleteSchedule(ScheduleModel schedule)
        {
            if (schedule == null)
                return;

            try
            {
                Season.Schedules.Remove(Season.Schedules.SingleOrDefault(x => x.ScheduleId == schedule.ScheduleId));
                IsLoading = true;
                await LeagueContext.UpdateModelAsync(Season);
                IsLoading = false;
                Load(Season);
            }
            catch
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public IEnumerator<ScheduleViewModel> GetEnumerator()
        {
            return ((IEnumerable<ScheduleViewModel>)ScheduleList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ScheduleViewModel>)ScheduleList).GetEnumerator();
        }
    }
}
