using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace iRLeagueManager.Models.Results
{
    public class ScoredTeamResultModel : ScoredResultModel
    {
        private ObservableCollection<ScoredTeamResultRowModel> teamResults;
        public ObservableCollection<ScoredTeamResultRowModel> TeamResults { get => teamResults; set => SetNotifyCollection(ref teamResults, value); }

        public ScoredTeamResultModel()
        {
            TeamResults = new ObservableCollection<ScoredTeamResultRowModel>();
        }
    }
}
