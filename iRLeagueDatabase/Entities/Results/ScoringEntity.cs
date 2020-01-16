using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iRLeagueManager.Enums;
using iRLeagueDatabase.Entities.Sessions;

namespace iRLeagueDatabase.Entities.Results
{
    public class ScoringEntity : Revision
    {
        [Key]
        public int ScoringId { get; set; }

        public override object MappingId => ScoringId;

        public int DropWeeks { get; set; }

        public int AverageRaceNr { get; set; }

        [ForeignKey(nameof(Schedule))]
        public int? ScheduleId { get; set; }
        public ScheduleEntity Schedule { get; set; }

        //[XmlArray("TotalScoringPoints")]
        //[XmlArrayItem("Entry")]
        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        ////public XmlDictionary<uint, int> XmlScorinPoints { get => (TotalScoringPoints != null) ? TotalScoringPoints : new Dictionary<uint, int>(); }
        //public XmlDictionaryRow<uint, int>[] XmlScoringPoints { get => TotalScoringPoints.Cast<XmlDictionaryRow<uint, int>>().ToArray(); set => TotalScoringPoints = value.ToDictionary(k => k.Key, v => v.Value); }

        public string ScoringRuleName { get; set; }

        //public ScoringRuleBase Rule { get; set; }

        public ScoringEntity() { }
    }
}
