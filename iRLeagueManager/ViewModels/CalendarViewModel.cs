// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
