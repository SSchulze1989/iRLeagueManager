using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.Models.Sessions
{
    public class ScheduleInfo : ModelBase, IScheduleInfo
    {
        [EqualityCheckProperty]
        public long? ScheduleId { get; internal set; }

        public override long? ModelId => ScheduleId;

        private string name;
        public string Name { get => name; set => SetValue(ref name, value); }

        private int sessionCount;
        public int SessionCount { get => sessionCount; set => SetValue(ref sessionCount, value); }

        public ScheduleInfo() : base()
        {
            ScheduleId = null;
        }

        public ScheduleInfo(long? scheduleId) : base()
        {
            ScheduleId = scheduleId;
        }
    }
}
