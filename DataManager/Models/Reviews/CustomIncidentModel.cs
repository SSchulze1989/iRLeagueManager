using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Reviews
{
    public class CustomIncidentModel : MappableModel
    {
        private long incidentId;
        public long IncidentId { get => incidentId; set => SetValue(ref incidentId, value); }

        private string text;
        public string Text { get => text; set => SetValue(ref text, value); }

        private int index;
        public int Index { get => index; set => SetValue(ref index, value); }

        public override long[] ModelId => new long[] { IncidentId };
    }
}
