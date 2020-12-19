using iRLeagueManager.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ViewModels
{
    public class StatisticSetViewModel : LeagueContainerModel<StatisticSetModel>
    {
        public long Id => Model.Id;
        public string Name => Model.Name;
        public TimeSpan UpdateInterval { get => Model.UpdateInterval; set => Model.UpdateInterval = value; }
        public DateTime UpdateTime { get => Model.Updatetime; set => Model.Updatetime = value; }

        protected override StatisticSetModel Template => new StatisticSetModel();
    }
}
