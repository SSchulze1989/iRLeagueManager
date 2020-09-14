using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using iRLeagueManager.Models.Members;

namespace iRLeagueManager.Models.Results
{
    public class TeamStandingsRowModel : StandingsRowModel
    {
        private TeamModel team;
        public TeamModel Team { get => team; set => SetValue(ref team, value); }

        private ObservableCollection<StandingsRowModel> driverStandingsRows;
        public ObservableCollection<StandingsRowModel> DriverStandingsRows { get => driverStandingsRows; set => SetValue(ref driverStandingsRows, value); }
    }
}
