using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Filters
{
    [DataContract]
    public class IntFilterValue : FilterValueBaseDTO
    {
        [DataMember]
        public int Value { get; set; }
    }
}
