using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Results
{
    public class AddPenaltyModel : ModelBase
    {
        public long? ScoredResultRowId { get; internal set; }
        public override long[] ModelId => new long[] { ScoredResultRowId.GetValueOrDefault() };
        private int penaltyPoints;
        public int PenaltyPoints { get => penaltyPoints; set => SetValue(ref penaltyPoints, value); }
        public AddPenaltyModel() { }

        public AddPenaltyModel(long? modelId)
        {
            ScoredResultRowId = modelId;
        }
    }
}
