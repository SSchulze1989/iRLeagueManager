using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.ViewModels.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace iRLeagueManager.ViewModels
{
    public class ScoringTableViewModel : LeagueContainerModel<ScoringTableModel>
    {
        public long ScoringTableId => (Model?.ScoringTableId).GetValueOrDefault();
        public string Name { get => Model.Name; set => Model.Name = value; }
        public int DropWeeks { get => Model.DropWeeks; set => Model.DropWeeks = value; }
        public int AverageRaceNr { get => Model.AverageRaceNr; set => Model.AverageRaceNr = value; }
        public ObservableCollection<MyKeyValuePair<ScoringInfo, double>> Scorings => Model.Scorings;
        public ObservableCollection<SessionInfo> Sessions => Model.Sessions;

        private SessionSelectViewModel sessionSelect;
        public SessionSelectViewModel SessionSelect
        {
            get
            {
                if (sessionSelect != null)
                    _ = sessionSelect.LoadSessions(Sessions);
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
                //_ = LoadStandings();
                return standings;
            }
        }

        private SeasonViewModel season;
        public SeasonViewModel Season { get => season; set => SetValue(ref season, value); }

        protected override ScoringTableModel Template => new ScoringTableModel();

        public ScoringTableViewModel()
        {
            SessionSelect = new SessionSelectViewModel()
            {
                SessionFilter = session => session.ResultAvailable
            };
            Model = Template;
        }

        protected async void OnSessionSelectChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SessionSelect.SelectedSession))
                await LoadStandings();
        }

        public async Task LoadStandings()
        {
            //await Task.Yield();
            try
            {
                //await standings.Load(ScoringId.GetValueOrDefault());
                if (SessionSelect?.SelectedSession != null)
                {
                    if (SessionSelect.FilteredSessions.Contains(SessionSelect.SelectedSession) == false)
                        SessionSelect.SelectedSession = SessionSelect.FilteredSessions.LastOrDefault();

                    await standings.Load(ScoringTableId, SessionSelect.SelectedSession.SessionId);
                }
                else
                    await standings.Load(ScoringTableId, 0);
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
    }
}
