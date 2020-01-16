using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer
{
    [DataContract]
    public abstract class VersionInfoDTO
    {
        public virtual Type Type { get => this.GetType(); }

        [DataMember]
        public DateTime? CreatedOn { get; set; } = null;
        [DataMember]
        public DateTime? LastModifiedOn { get; set; } = null;

        [DataMember]
        public int Version { get; set; }
    }
}
