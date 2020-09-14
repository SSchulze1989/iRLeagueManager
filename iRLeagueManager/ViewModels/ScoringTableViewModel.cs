using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.ViewModels.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using iRLeagueManager.Enums;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace iRLeagueManager.ViewModels
{
    public class ScoringTableViewModel : LeagueContainerModel<ScoringTableModel>
    {
        public long ScoringTableId => (Model?.ScoringTableId).GetValueOrDefault();
        public ScoringKindEnum ScoringKind { get => Model.ScoringKind; set => Model.ScoringKind = value; }
        public string Name { get => Model.Name; set => Model.Name = value; }
        public int DropWeeks { get => Model.DropWeeks; set => Model.DropWeeks = value; }
        public int AverageRaceNr { get => Model.AverageRaceNr; set => Model.AverageRaceNr = value; }
        public ObservableCollection<MyKeyValuePair<ScoringInfo, double>> Scorings => Model.Scorings;
        public ObservableCollection<SessionInfo> Sessions => Model.Sessions;
        public DropRacesOption DropRacesOption { get => Model.DropRacesOption; set => Model.DropRacesOption = value; }
        public int ResultsPerRaceCount { get => Model.ResultsPerRaceCount; set => Model.ResultsPerRaceCount = value; }

        private CollectionViewSource scoringListSource;
        public ICollectionView ScoringList
        {
            get
            {
                var view = scoringListSource.View;
                view.Filter = x => ((ScoringViewModel)x).ScoringKind == this.ScoringKind &&
                    Scorings.Any(y => y.Key.ScoringId == ((ScoringViewModel)x).ScoringId) == false;
                return view;
            }
        }

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

        //private StandingsViewModel standings;
        //public StandingsViewModel Standings
        //{
        //    get
        //    {
        //        if (standings == null)
        //            standings = new StandingsViewModel();
        //        return standings;
        //    }
        //}
        public StandingsViewModel Standings { get; } = new StandingsViewModel();

        private SeasonViewModel season;
        public SeasonViewModel Season { get => season; set => SetValue(ref season, value); }

        protected override ScoringTableModel Template => new ScoringTableModel();

        public ICommand AddScoringCmd { get; }
        public ICommand RemoveScoringCmd { get; }

        public ScoringTableViewModel()
        {
            SessionSelect = new SessionSelectViewModel()
            {
                SessionFilter = session => session.ResultAvailable
            };
            Model = Template;
            AddScoringCmd = new RelayCommand(o =>
            {
                if (o is IList selected)
                {
                    foreach (var scoring in selected.OfType<ScoringViewModel>().ToList())
                    {
                        AddScoring(scoring);
                    }
                }
                else
                {
                    AddScoring((ScoringViewModel)o);
                }
            }, o => o is ScoringViewModel || (o as IList)?.OfType<ScoringViewModel>().Count() > 0);
            RemoveScoringCmd = new RelayCommand(o =>
            {
                if (o is IList selected)
                {
                    foreach (var scoring in selected.OfType<MyKeyValuePair<ScoringInfo, double>>().ToList())
                    {
                        RemoveScoring(scoring.Key);
                    }
                }
                else
                {
                    RemoveScoring((ScoringInfo)o);
                }
            }, o => o is MyKeyValuePair<ScoringInfo, double> || (o as IList)?.OfType<MyKeyValuePair<ScoringInfo, double>>().Count() > 0);
        }

        public void SetScoringsList(ReadOnlyObservableCollection<ScoringViewModel> scoringList)
        {
            scoringListSource = new CollectionViewSource()
            {
                Source = scoringList
            };
            ScoringList.Refresh();
        }

        protected async void OnSessionSelectChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SessionSelect.SelectedSession))
                await LoadStandings();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            switch (propertyName)
            {
                case nameof(ScoringKind):
                    if (ScoringList.CanFilter)
                        ScoringList.Refresh();
                    break;
            }
            base.OnPropertyChanged(propertyName);
        }

        public void AddScoring(ScoringViewModel scoring)
        {
            if (Scorings.Any(x => x.Key.ScoringId == scoring.ScoringId) == false)
            {
                Scorings.Add(new MyKeyValuePair<ScoringInfo, double>(scoring.Model, 1));
            }
            if (ScoringList.CanFilter)
                ScoringList.Refresh();
        }

        public void RemoveScoring(ScoringInfo scoring)
        {
            var remove = Scorings.SingleOrDefault(x => x.Key.ScoringId == scoring.ScoringId);
            if (remove != null)
            {
                Scorings.Remove(remove);
            }
            if (ScoringList.CanFilter)
                ScoringList.Refresh();
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

                    await Standings.Load(ScoringTableId, SessionSelect.SelectedSession.SessionId);
                }
                else
                    await Standings.Load(ScoringTableId, 0);
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
