using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Statistics
{
    [DataContract]
    public class ImportedStatisticSetDTO : StatisticSetDTO
    {
        [DataMember]
        public string ImportSource { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime FirstDate { get; set; }
        [DataMember]
        public DateTime LastDate { get; set; }
    }
}