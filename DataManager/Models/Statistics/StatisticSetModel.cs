using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Statistics
{
    public class StatisticSetModel : StatisticSetInfo
    {
        private TimeSpan updateInterval;
        public TimeSpan UpdateInterval { get => updateInterval; set => SetValue(ref updateInterval, value); }

        private DateTime updatetime;
        public DateTime Updatetime { get => updatetime; set => SetValue(ref updatetime, value); }
    }
}
