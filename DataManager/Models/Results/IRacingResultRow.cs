using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iRLeagueManager.Models.Results
{
    public class IRacingResultRow : ResultRowModel
    {
        private string iracingId;
        public string IRacingId { get => iracingId; set { iracingId = value; OnPropertyChanged(); } }

        public IRacingResultRow() : base() { }
    }
}
