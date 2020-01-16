using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    public class ResultInfoDTO : VersionInfoDTO, IMappableDTO
    {
        [DataMember]
        public int ResultId { get; set; }

        object IMappableDTO.MappingId => ResultId;
    }
}
