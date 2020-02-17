using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iRLeagueManager.Enums;
using iRLeagueDatabase.Entities.Members;

namespace iRLeagueDatabase.Entities.Results
{
    [Serializable]
    public class ResultRowEntity : MappableEntity
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResultRowId { get; set; }

        public override object MappingId => ResultRowId;

        [Key, ForeignKey(nameof(Result)), Column(Order = 1)]
        public int? ResultId { get; set; }
        public virtual ResultEntity Result { get; set; }

        public DateTime? Date => Result?.Session?.Date;

        public int FinalPosition { get; set; }

        public int StartPosition { get; set; }

        public int FinishPosition { get; set; }

        //[ForeignKey(nameof(Member))]
        //public int MemberId { get; set; }
        public virtual LeagueMemberEntity Member { get; set; }
        
        public int CarNumber { get; set; }
        
        public int ClassId { get; set; }

        public string Car { get; set; }
        
        public string CarClass { get; set; }
        
        public int CompletedLaps { get; set; }
        
        public int LeadLaps { get; set; }
        
        public int FastLapNr { get; set; }
        
        public int Incidents { get; set; }
        
        public RaceStatusEnum Status { get; set; }
        
        public int RacePoints { get; set; }
        
        public int BonusPoints { get; set; }

        public int PenaltyPoints { get; set; }

        [NotMapped]
        public int TotalPoints => RacePoints + BonusPoints - PenaltyPoints;

        public long QualifyingTime { get; set; }

        public long Interval { get; set; }

        public long AvgLapTime { get; set; }

        public long FastestLapTime { get; set; }

        public int PositionChange { get; set; }

        public ResultRowEntity() { }
    }
}
