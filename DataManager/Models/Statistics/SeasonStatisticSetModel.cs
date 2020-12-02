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

        private ObservableCollection<ScoringInfo> scorings;
        public ObservableCollection<ScoringInfo> Scorings { get => scorings; set => SetNotifyCollection(ref scorings, value); }

        public SeasonStatisticSetModel()
        {
            Scorings = new ObservableCollection<ScoringInfo>();
        }
    }
}
