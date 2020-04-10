using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Reviews;

namespace iRLeagueManager.ViewModels
{
    public class ScoredResultViewModel : LeagueContainerModel<ScoredResultModel>
    {
        protected override ScoredResultModel Template => new ScoredResultModel();

        public ObservableModelCollection<ResultRowViewModel, ResultRowModel> RawResults => 
            new ObservableModelCollection<ResultRowViewModel, ResultRowModel>(Model?.RawResults);
        public ObservableCollection<IncidentReviewInfo> Reviews => Model?.Reviews;

        public ScoringInfo Scoring => Model.Scoring;
        public ObservableModelCollection<ScoredResultRowViewModel, ScoredResultRowModel> FinalResults =>
            new ObservableModelCollection<ScoredResultRowViewModel, ScoredResultRowModel>(Model?.FinalResults);
    }
}
