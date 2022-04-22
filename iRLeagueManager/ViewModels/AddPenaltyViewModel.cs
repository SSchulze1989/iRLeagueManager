using iRLeagueManager.Models.Results;
using iRLeagueManager.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ViewModels
{
    public class AddPenaltyViewModel : LeagueContainerModel<AddPenaltyModel>
    {
        public long? ScoredResultRowId => Model.ScoredResultRowId;

        public int PenaltyPoints { get => Model.PenaltyPoints; set => Model.PenaltyPoints = value; }

        public TimeSpan PenaltyTime { get => Model.PenaltyTime; set => Model.PenaltyTime = value;}
        public TimeComponentVector PenaltyTimeComponents { get; }
        protected override AddPenaltyModel Template => new AddPenaltyModel();

        public string Penalty
        {
            get
            {
                return PenaltyTime > TimeSpan.Zero ? $"{PenaltyTimeComponents.Minutes:00}:{PenaltyTimeComponents.Seconds:00}" : PenaltyPoints.ToString();
            }
            set
            {
                if (value.Contains(':'))
                {
                    var components = value.Split(':');
                    PenaltyTimeComponents.Seconds = int.TryParse(components[1], out int seconds) ? seconds : 0;
                    PenaltyTimeComponents.Minutes = int.TryParse(components[0], out int minutes) ? minutes : 0;
                    PenaltyPoints = 0;
                }
                else
                {
                    PenaltyPoints = int.TryParse(value, out int points) ? points : 0;
                    PenaltyTime = TimeSpan.Zero;
                }
            }
        }

        public AddPenaltyViewModel()
        {
            PenaltyTimeComponents = new TimeComponentVector(() => PenaltyTime, x => PenaltyTime = x);
        }
    }
}
