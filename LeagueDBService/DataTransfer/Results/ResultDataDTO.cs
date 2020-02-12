using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using iRLeagueDatabase.DataTransfer.Reviews;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Sessions;


namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    public class ResultDataDTO : ResultInfoDTO, IMappableDTO
    {
        public override Type Type => typeof(ResultDataDTO);

        //[DataMember]
        //public int ResultId { get; set; }

        //[DataMember]
        //public int SessionId { get; set; }
        [DataMember]
        public SeasonInfoDTO Season { get; set; }
        //[DataMember]
        //public SessionInfoDTO Session { get; set; }
        [DataMember]
        public IEnumerable<ResultRowDataDTO> RawResults { get; set; }
        [DataMember]
        public IEnumerable<IncidentReviewInfoDTO> Reviews { get; set; }
        //[DataMember]
        //public IEnumerable<ResultRowDataDTO> FinalResults { get; set; }

        [DataMember]
        public LeagueMemberInfoDTO CreatedBy { get; set; }
        [DataMember]
        public LeagueMemberInfoDTO LastModifiedBy { get; set; }

        public ResultDataDTO() {  }
    }
}
