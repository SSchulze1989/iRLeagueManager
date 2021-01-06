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
        private ObservableCollection<StatisticSetModel> statisticSets;
        public ObservableCollection<StatisticSetModel> StatisticSets { get => statisticSets; set => SetNotifyCollection(ref statisticSets, value); }

        public override string StatisticSetType => "League";

        public LeagueStatisticSetModel()
        {
            StatisticSets = new ObservableCollection<StatisticSetModel>();
        }
    }
}
