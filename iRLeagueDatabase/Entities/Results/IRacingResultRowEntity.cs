using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iRLeagueDatabase.Entities.Results
{
    public class IRacingResultRowEntity : ResultRowEntity
    {
        public string IRacingId { get; set; }

        public IRacingResultRowEntity() : base() { }
    }
}
