using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using iRLeagueManager.Models.Results;

namespace iRLeagueManager.ViewModels
{
    public class ScoredTeamResultViewModel : ScoredResultViewModel, IContainerModelBase<ScoredTeamResultModel>
    {
        public ObservableCollection<ScoredTeamResultRowModel> TeamResults => ((ScoredTeamResultModel)Model).TeamResults;

        public bool UpdateSource(ScoredTeamResultModel source)
        {
            return base.UpdateSource(source);
        }

        ScoredTeamResultModel IContainerModelBase<ScoredTeamResultModel>.GetSource()
        {
            return base.GetSource() as ScoredTeamResultModel;
        }
    }
}
