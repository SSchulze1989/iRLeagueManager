using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace iRLeagueManager.Models.Results
{
    public class ScoredResultModel : ResultModel
    {
        private ScoringInfo scoring;
        public ScoringInfo Scoring { get => scoring; set => SetValue(ref scoring, value); }

        private ObservableCollection<ScoredResultRowModel> finalResults;
        public ObservableCollection<ScoredResultRowModel> FinalResults { get => finalResults; set => SetNotifyCollection(ref finalResults, value); }
    }
}
