using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models.Results;

namespace iRLeagueManager.ViewModels
{
    public class ScoredResultRowViewModel : ResultRowViewModel
    {
        public override ResultRowModel Model
        {
            get
            {
                if (base.Model is ScoredResultRowModel model)
                    return model;
                return Template;
            }
            set => base.Model = value;
        }

        protected override ResultRowModel Template => new ScoredResultRowModel();

        public int RacePoints { get => ((ScoredResultRowModel)Model).RacePoints; set => ((ScoredResultRowModel)Model).RacePoints = value; }
        public int BonusPoints { get => ((ScoredResultRowModel)Model).BonusPoints; set => ((ScoredResultRowModel)Model).BonusPoints = value; }
        public int PenaltyPoints { get => ((ScoredResultRowModel)Model).PenaltyPoints; set => ((ScoredResultRowModel)Model).PenaltyPoints = value; }
        public int FinalPosition { get => ((ScoredResultRowModel)Model).FinalPosition; set => ((ScoredResultRowModel)Model).FinalPosition = value; }
    }
}
