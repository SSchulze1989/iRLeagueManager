using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using iRLeagueDatabase.DataTransfer.Members;

namespace iRLeagueDatabase.DataTransfer
{
    [DataContract]
    public abstract class VersionDTO : VersionInfoDTO
    {        
        [DataMember]
        public long? CreatedByUserId { get; set; }
        [DataMember]
        public long? LastModifiedByUserId { get; set; }
    }
}
