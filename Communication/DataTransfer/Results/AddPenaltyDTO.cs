using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Results
{
    public class AddPenaltyDTO : MappableDTO
    {
        public long? ScoredResultRowId { get; set; }
        public int PenaltyPoints { get; set; }

        public override object MappingId => ScoredResultRowId;

        public override object[] Keys => new object[] { ScoredResultRowId.GetValueOrDefault() };
    }
}
