﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using iRLeagueManager.Enums;
using iRLeagueDatabase.DataTransfer.Members;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    [KnownType(typeof(ResultInfoDTO))]
    public class ResultRowDataDTO : MappableDTO, IMappableDTO
    {
        [DataMember]
        public long? ResultRowId { get; set; }
        [DataMember]
        public long ResultId { get; set; }
        //[DataMember]
        //public int FinalPosition { get; set; }
        [DataMember]
        public int StartPosition { get; set; }
        [DataMember]
        public int FinishPosition { get; set; }
        [DataMember]
        //public int MemberId { get; set; }
        public LeagueMemberInfoDTO Member { get; set; }
        [DataMember]
        public int CarNumber { get; set; }
        [DataMember]
        public int ClassId { get; set; }
        [DataMember]
        public string Car { get; set; }
        [DataMember]
        public string CarClass { get; set; }
        [DataMember]
        public int CompletedLaps { get; set; }
        [DataMember]
        public int LeadLaps { get; set; }
        [DataMember]
        public int FastLapNr { get; set; }
        [DataMember]
        public int Incidents { get; set; }
        [DataMember]
        public RaceStatusEnum Status { get; set; }
        //[DataMember]
        //public int RacePoints { get; set; }
        //[DataMember]
        //public int BonusPoints { get; set; }
        [DataMember]
        public TimeSpan QualifyingTime { get; set; }
        [DataMember]
        public TimeSpan Interval { get; set; }
        [DataMember]
        public TimeSpan AvgLapTime { get; set; }
        [DataMember]
        public TimeSpan FastestLapTime { get; set; }
        [DataMember]
        public int PositionChange { get; set; }

        public override object MappingId => ResultRowId;

        //public override object[] Keys => new object[] { ResultRowId, ResultId };
        public override object[] Keys => new object[] { ResultRowId };

        public ResultRowDataDTO() { }
    }
}
