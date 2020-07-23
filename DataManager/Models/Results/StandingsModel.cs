using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using iRLeagueManager.LeagueDBServiceRef;
using iRLeagueManager.Models.Members;
using System.ComponentModel.DataAnnotations;

namespace iRLeagueManager.Models.Results
{
    public class StandingsModel : MappableModel
    {
        private ScoringInfo scoring;
        public ScoringInfo Scoring { get => scoring; internal set => SetValue(ref scoring, value); }

        private long? sessionId;
        public long? SessionId { get => sessionId; internal set => SetValue(ref sessionId, value); }

        public override long[] ModelId => new long[] { (Scoring?.ScoringId).GetValueOrDefault(), sessionId.GetValueOrDefault() };

        private List<StandingsRowModel> standingsRows;
        public List<StandingsRowModel> StandingsRows { get => standingsRows; internal set => SetValue(ref standingsRows, value); }

        private LeagueMember mostWinsDriver;
        public LeagueMember MostWinsDriver { get => mostWinsDriver; internal set => SetValue(ref mostWinsDriver, value); }

        private LeagueMember mostPolesDriver;
        public LeagueMember MostPolesDriver { get => mostPolesDriver; internal set => SetValue(ref mostPolesDriver, value); }

        private LeagueMember cleanestDriver;
        public LeagueMember CleanestDriver { get => cleanestDriver; internal set => SetValue(ref cleanestDriver, value); }

        private LeagueMember mostPenaltiesDriver;
        public LeagueMember MostPenaltiesDriver { get => mostPenaltiesDriver; internal set => SetValue(ref mostPenaltiesDriver, value); }

        public StandingsModel()
        {
            StandingsRows = new List<StandingsRowModel>();
        }
    }
}
