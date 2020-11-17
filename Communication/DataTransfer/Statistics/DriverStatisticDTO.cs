using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Statistics
{
    [DataContract]
    public class DriverStatisticDTO : MappableDTO
    {
        [DataMember]
        public long StatisticSetId { get; set; }
        [DataMember]
        public DriverStatisticRowDTO[] DriverStatisticRows { get; set; }

        public override object[] Keys => new object[] { StatisticSetId };
    }
}
