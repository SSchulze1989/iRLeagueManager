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

        private ScoredResultRowModel ScoredModel => Model as ScoredResultRowModel;

        protected override ResultRowModel Template => new ScoredResultRowModel();

        public int RacePoints { get => ScoredModel.RacePoints; set => ScoredModel.RacePoints = value; }
        public int BonusPoints { get => ScoredModel.BonusPoints; set => ScoredModel.BonusPoints = value; }
        public int PenaltyPoints { get => ScoredModel.PenaltyPoints; set => ScoredModel.PenaltyPoints = value; }
        public int FinalPosition { get => ScoredModel.FinalPosition; set => ScoredModel.FinalPosition = value; }
        public int TotalPoints { get => ScoredModel.TotalPoints; }
    }
}
