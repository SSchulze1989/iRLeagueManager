using iRLeagueDatabase.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Statistics
{
    [DataContract]
    public class StatisticSetDTO : VersionDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public TimeSpan UpdateInterval { get; set; }
        [DataMember]
        public DateTime UpdateTime { get; set; }

        public override object[] Keys => new object[] { Id };
        public override object MappingId => Id;
    }
}
