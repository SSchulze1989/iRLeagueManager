using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Interfaces;
using System.Collections.ObjectModel;
using iRLeagueManager.Attributes;
using System.ComponentModel;

namespace iRLeagueManager.ViewModels
{
    public class SeasonModel : ContainerModelBase<ISeason>, ISeason
    {
        [EqualityCheckProperty]
        public uint SeasonId => Source.SeasonId;

        [EqualityCheckProperty]
        public string SeasonName { get => Source.SeasonName; set { Source.SeasonName = value; NotifyPropertyChanged(); } }

        private ObservableModelCollection<ResultModel, IResult> _results;
        public ObservableModelCollection<ResultModel, IResult> Results { get => _results; set { _results = value; NotifyPropertyChanged(); } }
        IEnumerable<IResult> ISeason.Results => Source.Results;

        private ObservableModelCollection<ScheduleModel, ISchedule> _schedules;
        [NotifyParentProperty(true)]
        public ObservableModelCollection<ScheduleModel, ISchedule> Schedules { get => _schedules; set { _schedules = value; NotifyPropertyChanged(); } }
        IEnumerable<ISchedule> ISeason.Schedules => Source.Schedules;

        private ObservableModelCollection<ScoringModel, IScoring> _scorings;
        public ObservableModelCollection<ScoringModel, IScoring> Scorings { get => _scorings; set { _scorings = value; NotifyPropertyChanged(); } }
        IEnumerable<IScoring> ISeason.Scorings => Source.Scorings;

        public IEnumerable<ISession> Sessions => Source.Sessions;

        public int SessionCount => Source.SessionCount;

        public ISessionInfo LastSession => Source.LastSession;

        public IRaceInfo LastRace => Source.LastRace;

        public ObservableCollection<object> Children => new ObservableCollection<object> { Schedules, Results, Scorings };

        public SeasonModel(ISeason source) : base(source)
        {
            Schedules = new ObservableModelCollection<ScheduleModel, ISchedule>(source.Schedules, x => x.Season = this);
            Results = new ObservableModelCollection<ResultModel, IResult>(source.Results, x => x.Season = this);
            Scorings = new ObservableModelCollection<ScoringModel, IScoring>(source.Scorings, x => x.Season = this);
            NotifyPropertyChanged(null);
        }

        public override void UpdateSource(ISeason source)
        {
            base.UpdateSource(source);
            Schedules.UpdateSource(source.Schedules);
            Results.UpdateSource(source.Results);
            Scorings.UpdateSource(source.Scorings);
        }

        public IResult AddNewResult()
        {
            IResult result = Source.AddNewResult();
            Results.UpdateCollection();
            return result;
        }

        public ISchedule AddNewSchedule()
        {
            ISchedule schedule = Source.AddNewSchedule();
            Schedules.UpdateCollection();
            return schedule;
        }

        public IScoring AddNewScoring()
        {
            IScoring scoring = Source.AddNewScoring();
            NotifyPropertyChanged(nameof(Scorings));
            return scoring;
        }

        public ISession AddNewSession()
        {
            ISession session = Source.AddNewSession();
            NotifyPropertyChanged(nameof(session));
            return session;
        }
    }
}
