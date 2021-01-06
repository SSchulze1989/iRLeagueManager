using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Statistics
{
    public class StatisticSetInfo : MappableModel
    {
        public long Id { get; internal set; }

        public override long[] ModelId => new long[] { Id };
    }
}
