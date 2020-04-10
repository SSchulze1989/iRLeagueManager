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

        public override long? ModelId => ScoringId;
    }
}
