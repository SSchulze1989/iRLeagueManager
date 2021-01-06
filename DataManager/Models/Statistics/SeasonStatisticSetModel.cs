using iRLeagueManager.Models.Results;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Statistics
{
    public class SeasonStatisticSetModel : StatisticSetModel
    {
        private SeasonInfo season;
        public SeasonInfo Season { get => season; set => SetValue(ref season, value); }

        private ScoringTableModel scoringTable;
        public ScoringTableModel ScoringTable { get => scoringTable; set => SetValue(ref scoringTable, value); }

        public override string StatisticSetType => "Season";

        public SeasonStatisticSetModel()
        {
        }

        public SeasonStatisticSetModel(SeasonInfo season)
        {
            Season = season;
        }
    }
}
