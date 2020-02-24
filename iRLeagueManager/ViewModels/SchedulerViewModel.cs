using iRLeagueManager.Data;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Sessions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;
namespace iRLeagueManager.ViewModels
{
    public class SchedulerViewModel : ViewModelBase, INotifyPropertyChanged//, INotifyCollectionChanged, IEnumerable<ScheduleViewModel>
    {
        private LeagueContext LeagueContext => GlobalSettings.LeagueContext;

        private ObservableModelCollection<ScheduleViewModel, ScheduleModel> schedules;
        public ObservableModelCollection<ScheduleViewModel, ScheduleModel> Schedules
        {
            get => schedules;
            protected set
            {
                if (SetValue(ref schedules, value, (t, v) => t.GetSource().Equals(v.GetSource())))
                {
                    OnPropertyChanged(null);
                }
            }
        }

        private SeasonModel season;
        public SeasonModel Season { get => season; protected set => SetValue(ref season, value); }

        private ResultModel currentResult;
        public ResultModel CurrentResult { get => currentResult; set => SetValue(ref currentResult, value); }

        public ICommand UploadFileCmd { get; protected set; }
        public ICommand DeleteScheduleCmd { get; protected set; }
        public ICommand CreateScheduleCmd { get; protected set; }

        //public event NotifyCollectionChangedEventHandler CollectionChanged
        //{
        //    add
        //    {
        //        ((INotifyCollectionChanged)Schedules).CollectionChanged += value;
        //    }

        //    remove
        //    {
        //        ((INotifyCollectionChanged)Schedules).CollectionChanged -= value;
        //    }
        //}

        public SchedulerViewModel()
        {
            Schedules = new ObservableModelCollection<ScheduleViewModel, ScheduleModel>(new ScheduleModel[] { ScheduleModel.GetTemplate() });
            CreateScheduleCmd = new RelayCommand(o => CreateSchedule(), o => Season != null);

            UploadFileCmd = new RelayCommand(o =>
            {

            }, o => CurrentResult?.Session != null);
            //UploadFileCmd = new RelayCommand(o => { }, o => false);
            DeleteScheduleCmd = new RelayCommand(o => DeleteSchedule(o as ScheduleModel), o => o != null);
        }

        public async void CreateSchedule()
        {
            var newSchedule = new ScheduleModel()
            {
                Name = "New Schedule"
            };

            Season.Schedules.Add(newSchedule);
            try
            {
                IsLoading = true;
                await LeagueContext.UpdateModelAsync(Season);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
            Load(Season);
        }

        public async void Load(SeasonModel season)
        {
            var loadedModels = new List<ScheduleModel>();
            Season = season;
            if (season == null || season.Schedules.Count == 0)
            {
                Schedules.UpdateSource(new ScheduleModel[] { ScheduleModel.GetTemplate() });
                return;
            }
            else
            {
                loadedModels = Schedules.Select(x => x.GetSource()).Where(x => season.Schedules.Select(y => y.ScheduleId).Contains(x.ScheduleId)).ToList();
            }

            var newIds = season.Schedules.Select(x => x.ScheduleId.Value).Except(loadedModels.Select(x => x.ScheduleId.Value)).ToList();

            List<ScheduleModel> updateSchedules = new List<ScheduleModel>();

            try
            {
                IsLoading = true;
                if (loadedModels.Count() > 0)
                {
                    var add = await LeagueContext.UpdateModelsAsync(loadedModels);
                    updateSchedules.AddRange(add);
                }
                if (newIds.Count() > 0)
                {
                    var add = await LeagueContext.GetModelsAsync<ScheduleModel>(newIds);
                    updateSchedules.AddRange(add);
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

            //var scheduleIds = season.Schedules.Select(x => x.ScheduleId.Value);
            //schedules = await LeagueContext.GetModelsAsync<ScheduleModel>(scheduleIds);
            updateSchedules = updateSchedules.OrderBy(x => x.ScheduleId).ToList();

            Schedules.UpdateSource(updateSchedules);
            OnPropertyChanged(null);
        }

        public async void MoveSessionToSchedule(SessionModel session, ScheduleModel sourceSchedule, ScheduleModel targetSchedule)
        {
            if (session == null || targetSchedule == null || sourceSchedule == null)
                return;

            //SessionModel copySession;
            //if (session.SessionType == Enums.SessionType.Race)
            //    copySession = new RaceSessionModel(targetSchedule);
            //else
            //    copySession = new SessionModel(targetSchedule, session.SessionType);
            //copySession.CopyFrom(session);

            //if (session.SessionResult != null)
            //{
            //    var result = await LeagueContext.GetModelAsync<ResultModel>(session.SessionResult.ResultId.GetValueOrDefault());
            //    var copyResult = new ResultModel(copySession);
                
            //}
            targetSchedule.Sessions.Add(session);
            sourceSchedule.Sessions.Remove(session);

            try
            {
                await LeagueContext.UpdateModelAsync(targetSchedule);
                await LeagueContext.UpdateModelAsync(sourceSchedule);

                //await LeagueContext.UpdateModelAsync(Season);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }

            Load(Season);
        }
        public async void DeleteSchedule(ScheduleModel schedule)
        {
            if (schedule == null)
                return;

            if (MessageBox.Show("Do you really want to delete Schedule: " + schedule.Name + "?\nThis Action can not be undone. All Sessions, Results and Reviews associated with this Schedule will be Deleted!", "Delete Schedule?", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            try
            {
                schedule.Sessions.Clear();
                await LeagueContext.UpdateModelAsync(schedule);
                Season.Schedules.Remove(Season.Schedules.SingleOrDefault(x => x.ScheduleId == schedule.ScheduleId));
                IsLoading = true;
                await LeagueContext.UpdateModelAsync(Season);
                IsLoading = false;
                Load(Season);
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

        //public IEnumerator<ScheduleViewModel> GetEnumerator()
        //{
        //    return ((IEnumerable<ScheduleViewModel>)Schedules).GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return ((IEnumerable<ScheduleViewModel>)Schedules).GetEnumerator();
        //}

        public override void Refresh(string propertyName = "")
        {
            Load(Season);
            base.Refresh(propertyName);
        }
    }
}
