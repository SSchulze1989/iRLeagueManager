using iRLeagueManager.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ViewModels
{
    public class SeasonStatisticSetViewModel : StatisticSetViewModel, IContainerModelBase<SeasonStatisticSetModel>
    {
        public new SeasonStatisticSetModel Model => (SeasonStatisticSetModel)base.Model;

        public SeasonViewModel Season
        {
            get
            {
                if (Season.Model.SeasonId != Model.Season.SeasonId)
                {
                    _ = Season.Load(Model.Season.SeasonId.GetValueOrDefault());
                }
                return Season;
            }
        }

        public bool UpdateSource(SeasonStatisticSetModel source)
        {
            return base.UpdateSource(source);
        }

        SeasonStatisticSetModel IContainerModelBase<SeasonStatisticSetModel>.GetSource()
        {
            return Model;
        }


    }
}
