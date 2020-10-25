using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Filters
{
    [DataContract]
    public class FilterValueBaseDTO
    {
        public long FilterValueId { get; set; }
    }
}
