using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Sessions;

namespace iRLeagueManager.ViewModels
{
    public class ScoringViewModel : LeagueContainerModel<ScoringModel>
    {
        protected override ScoringModel Template => new ScoringModel();

        public long? ScoringId { get; internal set; }
        public string Name { get => Model.Name; set => Model.Name = value; }
        public int DropWeeks { get => Model.DropWeeks; set => Model.DropWeeks =value; }
        public int AverageRaceNr { get => Model.AverageRaceNr; set => Model.AverageRaceNr = value; }
        public ObservableCollection<SessionInfo> Sessions { get => Model.Sessions; }
        public long SeasonId { get => Model.SeasonId; set => Model.SeasonId = value; }
        public SeasonModel Season { get => Model.Season; set => Model.Season = value; }
        public ObservableCollection<KeyValuePair<int, int>> BasePoints => Model.BasePoints;
        public ObservableCollection<KeyValuePair<string, int>> BonusPoints => Model.BonusPoints;
        public ObservableCollection<KeyValuePair<int, int>> IncPenaltyPoints => Model.IncPenaltyPoints;
        public ObservableCollection<KeyValuePair<ScoringModel, double>> MultiScoringResults => Model.MultiScoringResults;
        public ObservableCollection<StandingsRowModel> Standings { get => Model.Standings; }

        public ScoringViewModel()
        {
            Model = Template;
        }
    }
}
