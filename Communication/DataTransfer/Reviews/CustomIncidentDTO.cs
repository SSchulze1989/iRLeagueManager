using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Reviews
{
    public class CustomIncidentDTO : MappableDTO
    {
        public long IncidentId { get; set; }
        public string Text { get; set; }
        public int Index { get; set; }

        public override object MappingId => IncidentId;
        public override object[] Keys => new object[] { IncidentId };
    }
}
