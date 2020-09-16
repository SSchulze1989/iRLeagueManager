using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iRLeagueDatabase.DataTransfer.Results
{
    public class IRacingResultRowDTO : ResultRowDataDTO
    {
        public string IRacingId { get; set; }

        public IRacingResultRowDTO() : base() { }
    }
}
