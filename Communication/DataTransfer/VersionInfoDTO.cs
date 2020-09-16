using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer
{
    [DataContract]
    public abstract class VersionInfoDTO : MappableDTO
    {
        public virtual Type Type { get => this.GetType(); }

        [DataMember]
        public DateTime? CreatedOn { get; set; } = null;
        [DataMember]
        public DateTime? LastModifiedOn { get; set; } = null;
        [DataMember]
        public string CreatedByUserId { get; set; }
        [DataMember]
        public string LastModifiedByUserId { get; set; }
        [DataMember]
        public string CreatedByUserName { get; set; }
        [DataMember]
        public string LastModifiedByUserName { get; set; }

        [DataMember]
        public int Version { get; set; }
    }
}
