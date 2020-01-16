using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using iRLeagueManager;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.ViewModels
{
    public class ScheduleViewModel : LeagueContainerModel<ScheduleModel>
    {
        public ScheduleModel Model
        {
            get => Source;
            private set
            {
                if (SetSource(value))
                {
                    //Sessions.UpdateSource(Model.Sessions);
                    OnPropertyChanged();
                }
            }
        }

        //public ObservableModelCollection<SessionViewModel, SessionModel> Sessions { get; } = new ObservableModelCollection<SessionViewModel, SessionModel>();

        public ObservableModelCollection<SessionViewModel, SessionModel> Sessions => new ObservableModelCollection<SessionViewModel, SessionModel>(Model?.Sessions);

        public int? ScheduleId => Model?.ScheduleId;

        public string Name { get => Model?.Name; set => Model.Name = value; }

        public int SessionCount => (Model?.SessionCount).Value;

        public ICommand AddSessionCmd { get; }
        public ICommand DeleteSessionsCmd { get; }

        private SessionViewModel selectedSession;
        public SessionViewModel SelectedSession { get => selectedSession; set => SetValue(ref selectedSession, value); }

        public ScheduleViewModel() : base()
        {
            Model = ScheduleModel.GetTemplate();
            Sessions.UpdateSource(new SessionModel[0]);
            AddSessionCmd = new RelayCommand(o => AddSession(), o => Model?.Sessions != null);
            DeleteSessionsCmd = new RelayCommand(o => DeleteSessions(o), o => SelectedSession != null);
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

        public async void Load(int scheduleId)
        {
            if (Model?.ScheduleId == null || Model.ScheduleId != scheduleId )
            {
                Model = ScheduleModel.GetTemplate();
            }

            try
            {
                IsLoading = true;
                Model = await LeagueContext.GetModelAsync<ScheduleModel>(scheduleId);
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

        //internal override bool UpdateSource(ScheduleModel source)
        //{
        //    var result = base.UpdateSource(source);
        //    Model = Source;
        //    return result;
        //}
    }
}
