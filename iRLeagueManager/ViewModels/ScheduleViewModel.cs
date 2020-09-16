using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;

using iRLeagueManager;
using iRLeagueManager.Models;
using iRLeagueManager.Data;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Services;
using iRLeagueManager.ViewModels.Collections;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace iRLeagueManager.ViewModels
{
    public class ScheduleViewModel : LeagueContainerModel<ScheduleModel>
    {
        private readonly ObservableModelCollection<SessionViewModel, SessionModel> sessions;
        public ObservableModelCollection<SessionViewModel, SessionModel> Sessions
        {
            get
            {
                if (sessions.GetSource() != Model?.Sessions)
                    sessions.UpdateSource(Model?.Sessions);
                return sessions;
            }
        }

        private bool expanded = true;
        public bool Expanded { get => expanded; set => SetValue(ref expanded, value); }

        public long? ScheduleId => Model?.ScheduleId;

        public string Name { get => Model?.Name; set => Model.Name = value; }

        public int SessionCount => (Model?.SessionCount).Value;

        public DateTime? ScheduleStart => (Sessions?.Count > 0) ? (DateTime?)Sessions.Min(x => x.Date) : null;

        public DateTime? ScheduleEnd => (Sessions?.Count > 0) ? (DateTime?)Sessions.Max(x => x.Date) : null;

        public ICommand AddSessionCmd { get; }
        public ICommand DeleteSessionsCmd { get; }
        public ICommand UploadFileCmd { get; protected set; }

        private SessionViewModel selectedSession;
        public SessionViewModel SelectedSession { get => selectedSession; set => SetValue(ref selectedSession, value); }

        protected override ScheduleModel Template => ScheduleModel.GetTemplate();

        public ScheduleViewModel() : base()
        {
            Model = ScheduleModel.GetTemplate();
            sessions = new ObservableModelCollection<SessionViewModel, SessionModel>(Model?.Sessions, x => x.Schedule = this);
            ((INotifyCollectionChanged)sessions).CollectionChanged += OnSessionCollectionChange;
            Sessions.UpdateSource(new SessionModel[0]);
            AddSessionCmd = new RelayCommand(o => AddSession(), o => Model?.Sessions != null && (!Model?.IsReadOnly).GetValueOrDefault());
            DeleteSessionsCmd = new RelayCommand(o => DeleteSessions(o), o => SelectedSession != null && (!Model?.IsReadOnly).GetValueOrDefault());
            UploadFileCmd = new RelayCommand(o => UploadFile(o as SessionModel), o => false);
        }

        public ScheduleViewModel(ScheduleModel source) : this()
        {
            Model = source;
        }

        public override void OnUpdateSource()
        {
            //if (Sessions != null)
            //    Sessions.UpdateSource(Model?.Sessions);
            //base.OnUpdateSource();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            switch(propertyName)
            {
                case nameof(Sessions):
                    //OnSessionCollectionChange(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    break;
            }
            base.OnPropertyChanged(propertyName);
        }

        private void OnSessionCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ScheduleStart));
            OnPropertyChanged(nameof(ScheduleEnd));
            OnPropertyChanged(nameof(SessionCount));
        }

        public void AddSession()
        {
            if (Model?.Sessions == null)
                return;

            var newSession = new RaceSessionModel(Model);
            AddSession(newSession);
        }

        public async void AddSession(SessionModel session)
        {
            Model.Sessions.Add(session);

            IsLoading = true;
            try
            {
                await LeagueContext.UpdateModelAsync(Model);
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

        public async void DeleteSessions(object selection)
        {
            if (Model == null)
                return;
            try
            {
                if (selection is SessionViewModel session) {
                    await LeagueContext.DeleteModelsAsync(session.GetSource());
                    //Model.Sessions.Remove(session.GetSource());
                }
                else if (selection is IEnumerable<SessionViewModel> sessions)
                {
                    await LeagueContext.DeleteModelsAsync(sessions.Select(x => x.GetSource()).ToArray());
                    foreach (var s in sessions)
                    {
                        Model.Sessions.Remove(s.GetSource());
                    }
                }
                else if (selection == null)
                {
                    await LeagueContext.DeleteModelsAsync(SelectedSession.GetSource());
                    Model.Sessions.Remove(SelectedSession.GetSource());
                }

                IsLoading = true;
                await LeagueContext.UpdateModelAsync(Model);
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

        protected override void Dispose(bool disposing)
        {
            Sessions.Dispose();
            base.Dispose(disposing);
        }

        //public override async void Load(long scheduleId)
        //{
        //    if (Model?.ScheduleId == null || Model.ScheduleId != scheduleId )
        //    {
        //        Model = ScheduleModel.GetTemplate();
        //    }

        //    try
        //    {
        //        IsLoading = true;
        //        Model = await LeagueContext.GetModelAsync<ScheduleModel>(scheduleId);
        //    }
        //    catch (Exception e)
        //    {
        //        GlobalSettings.LogError(e);
        //    }
        //    finally
        //    {
        //        IsLoading = false;
        //    }
        //}

        public async void UploadFile(SessionModel session)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "CSV Dateien (*.csv)|*.csv",
                Multiselect = false
            };
            if (openDialog.ShowDialog() == false)
            {
                return;
            }

            var fileName = openDialog.FileName;

            Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            ResultParserService parserService = new ResultParserService(GlobalSettings.LeagueContext);
            var lines = parserService.ParseCSV(new StreamReader(stream));
            stream.Dispose();

            //Update LeagueMember database
            var newMembers = parserService.GetNewMemberList(lines);
            foreach (var member in newMembers)
            {
                await GlobalSettings.LeagueContext.UpdateModelsAsync(newMembers);
            }
            //var sessionModel = season.GetSessions().SingleOrDefault(x => x.SessionId == session.SessionId);
            if (session == null)
                return;

            var resultRows = parserService.GetResultRows(lines);
            ResultModel result;
            if (session.SessionResult != null)
            {
                result = await LeagueContext.GetModelAsync<ResultModel>(session.SessionResult.ResultId.GetValueOrDefault());
                result.RawResults = new ObservableCollection<ResultRowModel>(resultRows);
                await GlobalSettings.LeagueContext.UpdateModelAsync(result);
            }
            else
            {
                //result = await GlobalSettings.LeagueContext.CreateResultAsync(sessionModel);
                result = new ResultModel(session);
                session.SessionResult = result;
                result.RawResults = new ObservableCollection<ResultRowModel>(resultRows);
                await GlobalSettings.LeagueContext.UpdateModelAsync(result);
                await GlobalSettings.LeagueContext.UpdateModelAsync(session);
            }
            //CurrentResult = await LeagueContext.GetModelAsync<ResultModel>(season.Results.OrderBy(x => x.Session.Date).LastOrDefault().ResultId);
        }

        //internal override bool UpdateSource(ScheduleModel source)
        //{
        //    var result = base.UpdateSource(source);
        //    Model = Source;
        //    return result;
        //}
    }
}
