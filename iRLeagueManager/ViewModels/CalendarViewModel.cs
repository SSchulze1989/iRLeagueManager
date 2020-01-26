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

namespace iRLeagueManager.ViewModels
{
    public class CalendarViewModel : SchedulerViewModel
    {
        private LeagueContext LeagueContext => GlobalSettings.LeagueContext;

        //private SeasonModel season;
        //public SeasonModel Season { get => season; set => SetValue(ref season, value);

        public IEnumerable<SessionViewModel> Sessions => this.Select(x => Sessions).Aggregate((x, y) => x.Concat(y)).OrderBy(x => x.Date);
        public IEnumerable<RaceSessionModel> Races => Sessions.Where(x => x.SessionType == Enums.SessionType.Race).Cast<RaceSessionModel>();

        public SessionViewModel NextRace => Sessions.FirstOrDefault(x => x.FullDate.CompareTo(DateTime.Now) > 0);
    }
}
