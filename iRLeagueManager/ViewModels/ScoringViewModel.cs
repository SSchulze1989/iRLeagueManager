using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Sessions;
using System.ComponentModel;
using iRLeagueManager.LeagueDBServiceRef;
using iRLeagueManager.ViewModels.Collections;
using System.Security.Policy;

namespace iRLeagueManager.ViewModels
{
    public class ScoringViewModel : LeagueContainerModel<ScoringModel>
    {
        protected override ScoringModel Template => new ScoringModel();
        public long? ScoringId => Model?.ScoringId; 
        public string Name { get => Model.Name; set => Model.Name = value; }
        public int DropWeeks { get => Model.DropWeeks; set => Model.DropWeeks =value; }
        public int AverageRaceNr { get => Model.AverageRaceNr; set => Model.AverageRaceNr = value; }
        public ObservableCollection<SessionInfo> Sessions { get => Model.Sessions; }
        public long SeasonId => Model.SeasonId;
        public SeasonModel Season { get => Model.Season; set => Model.Season = value; }
        public ObservableCollection<ScoringModel.BasePointsValue> BasePoints => Model.BasePoints;
        public ObservableCollection<ScoringModel.BonusPointsValue> BonusPoints => Model.BonusPoints;
        public ObservableCollection<ScoringModel.IncidentPointsValue> IncPenaltyPoints => Model.IncPenaltyPoints;
        public ObservableCollection<MyKeyValuePair<ScoringModel, double>> MultiScoringResults => Model.MultiScoringResults;

        private SessionSelectViewModel sessionSelect;
        public SessionSelectViewModel SessionSelect
        {
            get
            {
                return sessionSelect;
            }
            set
            {
                var temp = sessionSelect;
                if (SetValue(ref sessionSelect, value))
                {
                    if (temp != null)
                        temp.PropertyChanged -= OnSessionSelectChanged;
                    if (sessionSelect != null)
                        sessionSelect.PropertyChanged += OnSessionSelectChanged;
                }
            }
        }

        private StandingsViewModel standings = new StandingsViewModel();
        public StandingsViewModel Standings
        {
            get
            {
                if (standings == null)
                    standings = new StandingsViewModel();

                //_ = standings.Load(ScoringId.GetValueOrDefault(), SessionSelect.SelectedSession.SessionId);
                _ = LoadStandings();
                return standings;
            }
        }

        public ScheduleInfo ConnectedSchedule
        {
            get
            {
                var schedule = Season?.Schedules?.SingleOrDefault(x => x.ScheduleId == Model?.ConnectedSchedule?.ScheduleId);
                if (schedule == null)
                    schedule = Model.ConnectedSchedule;
                return schedule;
            }
            set => Model.ConnectedSchedule = value;
        }

        public ScoringViewModel()
        {
            SessionSelect = new SessionSelectViewModel();
            Model = Template;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName == nameof(Season) || propertyName == null)
            {
                if (Season != null && ConnectedSchedule != null && Season.Schedules.Any(x => x.ScheduleId == ConnectedSchedule.ScheduleId))
                {
                    ConnectedSchedule = Season.Schedules.SingleOrDefault(x => x.ScheduleId == ConnectedSchedule.ScheduleId);
                }
            }

            base.OnPropertyChanged(propertyName);
        }

        protected void OnSessionSelectChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        //public async Task LoadSessions()
        //{
            //ObservableModelCollection<SessionViewModel, SessionModel> sessionCollection = SessionSelect.SessionList as ObservableModelCollection<SessionViewModel, SessionModel>;

            //if (sessionCollection == null)
            //    SessionSelect.SessionList = sessionCollection = new ObservableModelCollection<SessionViewModel, SessionModel>();

            //IsLoading = true;
            //var sessionModels = await LeagueContext.GetModelsAsync<SessionModel>(Sessions.Select(x => x.ModelId));

            //var lastSelectedSession = SessionSelect.SelectedSession;

            //sessionCollection.UpdateSource(sessionModels.OrderBy(x => x.Date));

            //if (lastSelectedSession == null || !SessionSelect.SessionList.Contains(lastSelectedSession))
            //    SessionSelect.SelectedSession = SessionSelect.SessionList.Where(x => x.ResultAvailable).LastOrDefault();

            //await LoadStandings();
        //}

        public async Task LoadStandings()
        {
            try
            {
                await standings.Load(ScoringId.GetValueOrDefault());
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
            if (SessionSelect != null)
                SessionSelect.PropertyChanged -= OnSessionSelectChanged;

            base.Dispose(disposing);
        }
    }
}
