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
        public IEnumerable<SessionViewModel> Sessions => Schedules.Count() > 0 ? Schedules.SelectMany(x => x.Sessions).OrderBy(x => x.Date).ToList() : new List<SessionViewModel>();
        public IEnumerable<SessionViewModel> Races => Sessions.Where(x => x.SessionType == Enums.SessionType.Race);

        public SessionViewModel NextRace => Races.FirstOrDefault(x => x.FullDate.Date.Add(x.RaceEnd).CompareTo(DateTime.Now) > 0);

        private SessionViewModel selectedRace;
        public SessionViewModel SelectedRace { get => selectedRace; set => SetValue(ref selectedRace, value); }

        public IEnumerable<SessionViewModel> UpcomingSessions => Sessions.Where(x => x.FullDate >= DateTime.Now);

        public CalendarViewModel()
        {
            SelectedRace = Races.FirstOrDefault();
        }
    }
}
