using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using iRLeagueManager.Attributes;

namespace iRLeagueManager.Models.Results
{
    public class ScoredResultModel : ResultModel
    {
        public long? ScoredResultId { get; internal set; }

        private ScoringInfo scoring;
        public ScoringInfo Scoring { get => scoring; set => SetValue(ref scoring, value); }

        [EqualityCheckProperty]
        public long? ScoringId => Scoring?.ScoringId;

        private ObservableCollection<ScoredResultRowModel> finalResults;
        public ObservableCollection<ScoredResultRowModel> FinalResults { get => finalResults; set => SetNotifyCollection(ref finalResults, value); }

        public override long[] ModelId => new long[] { ResultId.GetValueOrDefault(), ScoringId.GetValueOrDefault() };
        //public override long[] ModelId => new long[] { ScoredResultId.GetValueOrDefault() };

        public  ScoredResultModel() : base()
        {
            FinalResults = new ObservableCollection<ScoredResultRowModel>();
        }

        public static ScoredResultModel GetTemplate()
        {
            var template = new ScoredResultModel();
            template.FinalResults.Add(ScoredResultRowModel.GetTemplate());

            return template;
        }
    }
}
