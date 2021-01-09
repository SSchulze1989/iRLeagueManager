// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using iRLeagueManager.Enums;
using iRLeagueManager.Models.Results;
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

        private bool filterOnlyPoints;
        public bool FilterPointsOnly { get => filterOnlyPoints; set => SetValue(ref filterOnlyPoints, value); }

        private ObservableCollection<FilterValueModel> filterValues;
        public ObservableCollection<FilterValueModel> FilterValues { get => filterValues; set => SetNotifyCollection(ref filterValues, value); }

        public Type ColumnPropertyType => typeof(ResultRowModel).GetProperty(ColumnPropertyName ?? "")?.PropertyType;

        public override long[] ModelId => new long[] { ResultsFilterId };

        public ResultsFilterOptionModel() 
        {
            FilterValues = new ObservableCollection<FilterValueModel>();
        }

        public ResultsFilterOptionModel(long scoringId) : this()
        {
            ScoringId = scoringId;
        }

        public ResultsFilterOptionModel(long resultsFilterId, long scoringId) : this(scoringId)
        {
            ResultsFilterId = resultsFilterId;
        }
    }
}
