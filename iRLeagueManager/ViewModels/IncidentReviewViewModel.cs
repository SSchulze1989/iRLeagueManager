using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models.Reviews;
using iRLeagueManager.ViewModels.Collections;

namespace iRLeagueManager.ViewModels
{
    public class IncidentReviewViewModel : LeagueContainerModel<IncidentReviewModel>
    {
        //public IncidentReviewModel Model { get => Source; set => SetSource(value); }

        private readonly ObservableModelCollection<ReviewCommentViewModel, ReviewCommentModel> comments;
        public ObservableModelCollection<ReviewCommentViewModel, ReviewCommentModel> Comments
        {
            get
            {
                if (comments.GetSource() != Model?.Comments)
                    comments.UpdateSource(Model?.Comments);
                return comments;
            }
        }

        protected override IncidentReviewModel Template => new IncidentReviewModel();
        //...

        public IncidentReviewViewModel()
        {
            comments = new ObservableModelCollection<ReviewCommentViewModel, ReviewCommentModel>(x => x.Review = this);
        }
    }
}
