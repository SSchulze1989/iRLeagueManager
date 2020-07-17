using iRLeagueManager.Data;
using iRLeagueManager.Enums;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.ViewModels.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace iRLeagueManager.ViewModels
{
    public class ReviewCommentViewModel : CommentViewModel, IContainerModelBase<ReviewCommentModel>
    {
        public new ReviewCommentModel Model => base.Model as ReviewCommentModel;
        
        public ObservableModelCollection<CommentViewModel, CommentBase> replys;
        public ObservableModelCollection<CommentViewModel, CommentBase> Replys
        {
            get
            {
                if (replys.GetSource() != Model?.Replies)
                    replys.UpdateSource(Model?.Replies);
                return replys;
            }
        }
        public ObservableCollection<ReviewVoteModel> Votes => Model?.CommentReviewVotes;

        public ICommand ReplyCmd { get; private set; }

        protected override CommentBase Template => new ReviewCommentModel(new UserModel(0) { UserName = "TestAuthor" })
        {
            Text = "Test comment, Kat0 please!\nWith line break, yeah!",
            Replies = new ObservableCollection<CommentBase>(new List<CommentBase>
            {
                new CommentBase(new UserModel(0) { UserName = "MemberTwo" })
                {
                    Text = "This is a reply!"
                }
            }),
            CommentReviewVotes = new ObservableCollection<ReviewVoteModel>(new List<ReviewVoteModel>()
            {
                new ReviewVoteModel() { Vote = VoteEnum.Kat1, MemberAtFault = new LeagueMember(0, "Bad", "Driver") }
            })
        };

        public ReviewCommentViewModel()
        {
            ReplyCmd = new RelayCommand(o => { }, o => false);
            replys = new ObservableModelCollection<CommentViewModel, CommentBase>();
            SetSource(Template);
        }

        public ReviewCommentViewModel(ReviewCommentModel comment) : base(comment) { }

        public new ReviewCommentModel GetSource()
        {
            return base.GetSource() as ReviewCommentModel;
        }

        public new bool UpdateSource(ReviewCommentModel source)
        {
            return base.UpdateSource(source);
        }

        public async Task<CommentBase> ReplyAsync(string text)
        {
            if (Model == null)
                return null;

            var author = LeagueContext.UserManager.CurrentUser;
            if (author == null)
                return null;

            var newComment = new CommentBase(author, Model) { Text = text };

            try
            {
                IsLoading = true;
                newComment = await LeagueContext.AddModelAsync(newComment);
                await LeagueContext.UpdateModelAsync(Model);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }

            return newComment;
        }
    }
}
