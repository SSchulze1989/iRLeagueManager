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
        public override long[] ModelId => Team.ModelId;

        private TeamModel team;
        public TeamModel Team { get => team; set => SetValue(ref team, value); }

        private IEnumerable<StandingsRowModel> driverStandingsRows;
        public IEnumerable<StandingsRowModel> DriverStandingsRows { get => driverStandingsRows.OrderBy(x => -x.TotalPoints); set => SetValue(ref driverStandingsRows, value); }
    }
}
