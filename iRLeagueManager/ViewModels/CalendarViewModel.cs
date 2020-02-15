using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using iRLeagueManager.Data;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace iRLeagueManager.ViewModels
{
    public class CalendarViewModel : SchedulerViewModel
    {
        private LeagueContext LeagueContext => GlobalSettings.LeagueContext;

        //private SeasonModel season;
        //public SeasonModel Season { get => season; set => SetValue(ref season, value);

        public IEnumerable<SessionViewModel> Sessions => Schedules.Select(x => x.Sessions.AsEnumerable()).Aggregate((x, y) => x.Concat(y));
        public IEnumerable<SessionViewModel> Races => Sessions.Where(x => x.SessionType == Enums.SessionType.Race);

        public SessionViewModel NextRace => Races.FirstOrDefault(x => x.FullDate.CompareTo(DateTime.Now) > 0);

        private SessionViewModel selectedRace;
        public SessionViewModel SelectedRace { get => selectedRace; set => SetValue(ref selectedRace, value); }

        public IEnumerable<SessionViewModel> UpcomingSessions => Sessions.Where(x => x.FullDate >= DateTime.Now);

        public CalendarViewModel()
        {
            SelectedRace = Races.FirstOrDefault();
        }
    }
}
