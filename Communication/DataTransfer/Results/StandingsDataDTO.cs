using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueDatabase.DataTransfer.Members;

namespace iRLeagueDatabase.DataTransfer.Results
{
    public class StandingsDataDTO : MappableDTO
    {
        public ScoringInfoDTO Scoring { get; set; }
        public long ScoringTableId { get; set; }
        public override object MappingId => new long[] { Scoring.ScoringId.GetValueOrDefault() };
        public long? SessionId { get; set; }
        public override object[] Keys => new object[] { Scoring.ScoringId.GetValueOrDefault() };
        public virtual StandingsRowDataDTO[] StandingsRows { get; set; }
        public virtual LeagueMemberInfoDTO MostWinsDriver { get; set; }
        public virtual LeagueMemberInfoDTO MostPolesDriver { get; set; }
        public virtual LeagueMemberInfoDTO CleanestDriver { get; set; }
        public virtual LeagueMemberInfoDTO MostPenaltiesDriver { get; set; }
    }
}
