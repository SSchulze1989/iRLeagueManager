using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models.Reviews;

namespace iRLeagueManager.ViewModels
{
    public class IncidentReviewViewModel : LeagueContainerModel<IncidentReviewModel>
    {
        //public IncidentReviewModel Model { get => Source; set => SetSource(value); }

        protected override IncidentReviewModel Template => new IncidentReviewModel();
        //...
    }
}
