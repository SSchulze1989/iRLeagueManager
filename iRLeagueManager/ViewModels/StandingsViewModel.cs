using iRLeagueManager.Models.Results;
using iRLeagueManager.ViewModels.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.ViewModels
{
    public class StandingsViewModel : LeagueContainerModel<StandingsModel>
    {
        //ScoringInfo Scoring => Model?.Scoring;
        public bool IsTeamStandings => Model is TeamStandingsModel;
        public IEnumerable<StandingsRowModel> StandingsRows => Model?.StandingsRows.OrderBy(x => -x.TotalPoints);
        public LeagueMember MostWinsDriver => Model?.MostWinsDriver;
        public LeagueMember MostPolesDriver => Model?.MostPolesDriver;
        public LeagueMember CleanestDriver => Model?.CleanestDriver;
        public LeagueMember MostPenaltiesDriver => Model?.MostPenaltiesDriver;

        protected override StandingsModel Template => new StandingsModel();

        public override Task Load(params long[] modelId)
        {
            if (Model == null || Model.ModelId.SequenceEqual(modelId) == false)
                return base.Load(modelId);
            else
                return base.Update();
        }
    }
}
