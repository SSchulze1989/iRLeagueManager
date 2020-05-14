using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Results
{
    public class ScoringInfo : ModelBase
    {
        public long? ScoringId { get; internal set; }

        public ScoringInfo() : base()
        {
            ScoringId = null;
        }
        public ScoringInfo(long? scoringId) : base()
        {
            ScoringId = scoringId;
        }

        public override long[] ModelId => new long[] { ScoringId.GetValueOrDefault() };
    }
}
