using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Statistics
{
    public class DriverStatisticModel : MappableModel
    {
        private long statisticSetId;
        public long StatisticSetId { get => statisticSetId; set => SetValue(ref statisticSetId, value); }
        
        private DriverStatisticRowModel[] driverStatisticRows;
        public DriverStatisticRowModel[] DriverStatisticRows { get => driverStatisticRows; set => SetValue(ref driverStatisticRows, value); }

        public override long[] ModelId => new long[] { StatisticSetId };
    }
}
