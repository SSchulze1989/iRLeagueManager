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

namespace iRLeagueManager.ViewModels
{
    public class ScheduleViewModel : LeagueContainerModel<ScheduleModel>
    {
        //public ScheduleModel Model
        //{
        //    get => Source;
        //    private set
        //    {
        //        if (SetSource(value))
        //        {
        //            //Sessions.UpdateSource(Model.Sessions);
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //public ObservableModelCollection<SessionViewModel, SessionModel> Sessions { get; } = new ObservableModelCollection<SessionViewModel, SessionModel>();

        public ObservableModelCollection<SessionViewModel, SessionModel> Sessions => new ObservableModelCollection<SessionViewModel, SessionModel>(Model?.Sessions, x => x.Schedule = this);

        public long? ScheduleId => Model?.ScheduleId;

        public string Name { get => Model?.Name; set => Model.Name = value; }

        public int SessionCount => (Model?.SessionCount).Value;

        public ICommand AddSessionCmd { get; }
        public ICommand DeleteSessionsCmd { get; }
        public ICommand UploadFileCmd { get; protected set; }

        private SessionViewModel selectedSession;
        public SessionViewModel SelectedSession { get => selectedSession; set => SetValue(ref selectedSession, value); }

        protected override ScheduleModel Template => ScheduleModel.GetTemplate();

        public ScheduleViewModel() : base()
        {
            Model = ScheduleModel.GetTemplate();
            Sessions.UpdateSource(new SessionModel[0]);
            AddSessionCmd = new RelayCommand(o => AddSession(), o => Model?.Sessions != null);
            DeleteSessionsCmd = new RelayCommand(o => DeleteSessions(o), o => SelectedSession != null);
            UploadFileCmd = new RelayCommand(o => UploadFile(o as SessionModel), o => false);
        }

        public ScheduleViewModel(ScheduleModel source) : this()
        {
            Model = source;
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

            if (selection is SessionViewModel session) {
                Model.Sessions.Remove(session.GetSource());
            }
            else if (selection is IEnumerable<SessionViewModel> sessions)
            {
                foreach (var s in sessions)
                {
                    Model.Sessions.Remove(s.GetSource());
                }
            }
            else if (selection == null)
            {
                Model.Sessions.Remove(SelectedSession.GetSource());
            }

            await LeagueContext.UpdateModelAsync(Model);
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
