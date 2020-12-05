using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Statistics
{
    public class LeagueStatisticSetModel : StatisticSetModel
    {
        private ObservableCollection<StatisticSetInfo> seasonStatisticSets;
        public ObservableCollection<StatisticSetInfo> SeasonStatisticSets { get => seasonStatisticSets; set => SetValue(ref seasonStatisticSets, value); }

        public LeagueStatisticSetModel()
        {
            SeasonStatisticSets = new ObservableCollection<StatisticSetInfo>();
        }
    }
}
