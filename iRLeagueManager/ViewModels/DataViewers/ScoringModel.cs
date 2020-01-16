using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.ViewModels
{
    public class ScoringModel : ContainerModelBase<IScoring>, IScoring
    {
        public SeasonModel Season { get; internal set; }
        public int ScoringNr => Source.ScoringNr;

        public int DropWeeks { get => Source.DropWeeks; set { Source.DropWeeks = value; NotifyPropertyChanged(); } }
        public int AverageRaceNr { get => Source.AverageRaceNr; set { Source.AverageRaceNr = value; NotifyPropertyChanged(); } }
        
        public ScheduleModel Schedule
        {
            get
            {
                if (Source.Schedule is ScheduleModel)
                {
                    return Source.Schedule as ScheduleModel;
                }
                else
                {
                    return Season.Schedules.SingleOrDefault(x => x.ScheduleId == Source.Schedule.ScheduleId);
                }
            }
            set
            {
                Source.Schedule = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Races));
            }
        }
        ISchedule IScoring.Schedule { get => Source.Schedule; set => Source.Schedule = value; }

        //public ISchedule Schedule { get => Source.Schedule; set { Source.Schedule = value; NotifyPropertyChanged(); } }
        public ObservableModelCollection<RaceSessionModel, ISession> Races => Schedule.RaceSessions;
        IEnumerable<IRaceSession> IScoring.Races => Races;
        //public IEnumerable<IRaceSession> Races => Source.Races;

        public ScoringModel() : base() { }
        public ScoringModel(IScoring source) : base(source) { }
    }
}
