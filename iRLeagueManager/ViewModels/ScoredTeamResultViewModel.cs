using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections;
using System.Windows.Data;
using System.Collections.ObjectModel;

using iRLeagueManager.Models.Results;
using iRLeagueManager.ViewModels.Collections;

namespace iRLeagueManager.ViewModels
{
    public class ScoredTeamResultViewModel : ScoredResultViewModel, IContainerModelBase<ScoredTeamResultModel>
    {
        public new ScoredTeamResultModel Model { get => base.Model as ScoredTeamResultModel; set => base.Model = value; }

        private readonly ObservableModelCollection<ScoredTeamResultRowViewModel, ScoredTeamResultRowModel> teamResults;
        public ObservableModelCollection<ScoredTeamResultRowViewModel, ScoredTeamResultRowModel> TeamResults
        {
            get
            {
                if (teamResults.GetSource() != Model?.TeamResults)
                    teamResults.UpdateSource(Model?.TeamResults);
                return teamResults;
            }
        }

        public ScoredTeamResultViewModel()
        {
            teamResults = new ObservableModelCollection<ScoredTeamResultRowViewModel, ScoredTeamResultRowModel>();
        }

        public bool UpdateSource(ScoredTeamResultModel source)
        {
            return base.UpdateSource(source);
        }

        public override void OnUpdateSource()
        {
            base.OnUpdateSource();
        }

        ScoredTeamResultModel IContainerModelBase<ScoredTeamResultModel>.GetSource()
        {
            return base.GetSource() as ScoredTeamResultModel;
        }
    }
}
