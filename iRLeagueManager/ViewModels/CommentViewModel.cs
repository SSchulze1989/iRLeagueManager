using iRLeagueManager.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using iRLeagueManager.Models.Members;

namespace iRLeagueManager.ViewModels
{
    public class CommentViewModel : LeagueContainerModel<CommentBase>
    {
        public long CommentId => (Model?.CommentId).GetValueOrDefault();

        private IncidentReviewViewModel review;
        public IncidentReviewViewModel Review { get => review; set => SetValue(ref review, value); }
        public LeagueMember Author => Model?.Author;
        public string Text { get => Model?.Text; set => Model.Text = value; }
        public DateTime Date => (Model?.Date).GetValueOrDefault();
        protected override CommentBase Template => new ReviewCommentModel(new LeagueMember(0, "User", "Two"))
        {
            Text = "This is a reply!\nAlso with a line break!"
        };
        public bool IsUserAuthor => (LeagueContext.CurrentUser?.MemberId).GetValueOrDefault() == Author.MemberId.GetValueOrDefault();

        public ICommand EditCmd { get; private set; }

        public CommentViewModel() : this(null)
        {
            SetSource(Template);
        }
        
        public CommentViewModel(CommentBase source) : base(source) 
        {
            EditCmd = new RelayCommand(async o => await EditAsync(o as string), o => IsUserAuthor);
        }

        public async Task<bool> EditAsync(string editedText)
        {
            string oldText = Text;
            bool status = false;

            try
            {
                if (LeagueContext.CurrentUser.MemberId != Author.MemberId)
                    throw new UnauthorizedAccessException("Can not edit Comment text. Insufficient privileges!");

                IsLoading = true;
                Text = editedText;
                await LeagueContext.UpdateModelAsync(Model);
                status = true;
            }
            catch (Exception e)
            {
                Text = oldText;
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }

            return status;
        }
    }
}
