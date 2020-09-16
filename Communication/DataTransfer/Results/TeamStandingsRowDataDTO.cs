using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Results
{
    public class TeamStandingsRowDataDTO : StandingsRowDataDTO
    {
        public long TeamId { get; set; }
        public StandingsRowDataDTO[] DriverStandingsRows { get; set; }
    }
}
