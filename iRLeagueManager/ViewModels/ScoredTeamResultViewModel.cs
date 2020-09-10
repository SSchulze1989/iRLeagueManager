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

namespace iRLeagueManager.ViewModels
{
    public class ScoredTeamResultViewModel : ScoredResultViewModel, IContainerModelBase<ScoredTeamResultModel>
    {
        public new ScoredTeamResultModel Model { get => base.Model as ScoredTeamResultModel; set => base.Model = value; }
        public ObservableCollection<ScoredTeamResultRowModel> TeamResults => Model?.TeamResults;

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
