using iRLeagueManager.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Filters
{
    [DataContract]
    public class ResultsFilterOptionDTO : MappableDTO
    {
        [DataMember]
        public long ResultsFilterId { get; set; }
        [DataMember]
        public long ScoringId { get; set; }
        [DataMember]
        public string ResultsFilterType { get; set; }
        [DataMember]
        public string ColumnPropertyName { get; set; }
        [DataMember]
        public ComparatorTypeEnum Comparator { get; set; }
        [DataMember]
        public bool Exclude { get; set; }
        [DataMember]
        public object[] FilterValues { get; set; }

        public override object MappingId => ResultsFilterId;

        public override object[] Keys => new object[] { ResultsFilterId };
    }
}
