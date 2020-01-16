using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using iRLeagueDatabase.DataTransfer.Sessions;
using iRLeagueDatabase.DataTransfer.Results;
using iRLeagueDatabase.DataTransfer.Reviews;
using System.ServiceModel;
using System.Runtime.Serialization;
using iRLeagueDatabase.DataTransfer.Members;

namespace iRLeagueDatabase.DataTransfer
{
    [DataContract]
    public class SeasonDataDTO : SeasonInfoDTO
    {
        public override Type Type => typeof(SeasonDataDTO);

        //[DataMember]
        //public int SeasonId { get; set; }

        [DataMember]
        public IEnumerable<ScheduleInfoDTO> Schedules { get; set; }

        //public virtual List<ScoringDataDTO> Scorings { get; set; }
        [DataMember]
        public IEnumerable<IncidentReviewInfoDTO> Reviews { get; set; }

        [DataMember]
        public IEnumerable<ResultInfoDTO> Results { get; set; }

        [DataMember]
        public DateTime SeasonStart { get; set; }

        [DataMember]
        public DateTime SeasonEnd { get; set; }

        //public ScoringDataDTO MainScoring { get; set; }

        [DataMember]
        public LeagueMemberInfoDTO CreatedBy { get; set; }
        [DataMember]
        public LeagueMemberInfoDTO LastModifiedBy { get; set; }

        public SeasonDataDTO() { }
    }
}
