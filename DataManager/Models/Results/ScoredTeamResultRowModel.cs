using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using iRLeagueManager.Models.Members;

namespace iRLeagueManager.Models.Results
{
    public class ScoredTeamResultRowModel : ScoredResultRowModel
    {
        private TeamModel team;
        public TeamModel Team { get => team; internal set => SetValue(ref team, value); }

        private ObservableCollection<ScoredResultRowModel> scoredResultRows;
        public ObservableCollection<ScoredResultRowModel> ScoredResultRows { get => scoredResultRows; internal set => SetNotifyCollection(ref scoredResultRows, value); }
    }
}
