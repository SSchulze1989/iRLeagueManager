using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using iRLeagueDatabase.DataTransfer.Members;

namespace iRLeagueDatabase.DataTransfer
{
    //[DataContract]
    public interface IMappableDTO
    {
        //[IgnoreDataMember]
        bool IsReadOnly { get; set; }
        object MappingId { get; }
        object[] Keys { get; }
    }
}
