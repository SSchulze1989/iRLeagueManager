using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    public class ScoredResultDataDTO : ResultInfoDTO, IMappableDTO
    {
        //[DataMember]
        //public long? ScoredResultId { get; set; }
        [DataMember]
        public ScoringInfoDTO Scoring { get; set; }

        [DataMember]
        public string ScoringName { get; set; }

        public override object MappingId => new long[] { ResultId.GetValueOrDefault(), (Scoring?.ScoringId).GetValueOrDefault() };

        public override object[] Keys => new object[] { ResultId.GetValueOrDefault(), (Scoring?.ScoringId).GetValueOrDefault() };

        public override Type Type => typeof(ScoredResultDataDTO);
        //object IMappableDTO.MappingId => MappingId;
        [DataMember]
        public IEnumerable<ScoredResultRowDataDTO> FinalResults { get; set; }
    }
}
