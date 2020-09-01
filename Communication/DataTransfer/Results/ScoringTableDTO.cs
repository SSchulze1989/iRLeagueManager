using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using iRLeagueDatabase.DataTransfer.Sessions;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    public class ScoringTableDataDTO : ScoringTableInfoDTO
    {
        [DataMember]
        public int DropWeeks { get; set; }
        [DataMember]
        public int AverageRaceNr { get; set; }
        [DataMember]
        public SeasonInfoDTO Season { get; set; }
        [DataMember]
        public string ScoringFactors { get; set; }
        [DataMember]
        public ScoringInfoDTO[] Scorings { get; set; }
        [DataMember]
        public SessionInfoDTO[] Sessions { get; set; }
    }
}
