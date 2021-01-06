using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ViewModels
{
    public class SeasonStatisticSetViewModel : StatisticSetViewModel, IContainerModelBase<SeasonStatisticSetModel>
    {
        public new SeasonStatisticSetModel Model => (SeasonStatisticSetModel)base.Model;

        public ScoringTableModel ScoringTable 
        { 
            get => Model.ScoringTable; 
            set => Model.ScoringTable = value; 
        }

        private readonly SeasonViewModel season;
        public SeasonViewModel Season
        {
            get
            {
                if (season.Model.SeasonId != Model.Season.SeasonId)
                {
                    _ = season.Load(Model.Season.SeasonId.GetValueOrDefault());
                }
                return season;
            }
        }

        public override string ShortDescription => $"{base.ShortDescription} ({Season.SeasonName})";

        protected override StatisticSetModel Template => new SeasonStatisticSetModel();

        public SeasonStatisticSetViewModel()
        {
            season = new SeasonViewModel();
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
