﻿using iRLeagueManager.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Filters
{
    public class ResultsFilterOptionModel : MappableModel
    {
        public long ResultsFilterId { get; internal set; }
        public long ScoringId { get; internal set; }

        private string resultsFilterType;
        public string ResultsFilterType { get => resultsFilterType; set => SetValue(ref resultsFilterType, value); }

        private string columnPropertyName;
        public string ColumnPropertyName { get => columnPropertyName; set => SetValue(ref columnPropertyName, value); }

        private ComparatorTypeEnum comparator;
        public ComparatorTypeEnum Comparator { get => comparator; set => SetValue(ref comparator, value); }

        private bool exclude;
        public bool Exclude { get => exclude; set => SetValue(ref exclude, value); }

        private ObservableCollection<object> filterValues;
        public ObservableCollection<object> FilterValues { get => filterValues; set => SetNotifyCollection(ref filterValues, value); }

        public override long[] ModelId => new long[] { ScoringId, ResultsFilterId };

        public ResultsFilterOptionModel() { }

        public ResultsFilterOptionModel(long scoringId)
        {
            ScoringId = scoringId;
        }

        public ResultsFilterOptionModel(long resultsFilterId, long scoringId) : this(scoringId)
        {
            ResultsFilterId = resultsFilterId;
        }
    }
}