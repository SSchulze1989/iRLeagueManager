using iRLeagueManager.Data;
using iRLeagueManager.Enums;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.ViewModels.Collections;
using iRLeagueManager.Models.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace iRLeagueManager.ViewModels
{
    public class ReviewCommentViewModel : CommentViewModel, IContainerModelBase<ReviewCommentModel>
    {
        public new ReviewCommentModel Model => base.Model as ReviewCommentModel;

        private IncidentReviewViewModel review;
        public IncidentReviewViewModel Review { get => review; set => SetValue(ref review, value); }
        
        public ObservableModelCollection<CommentViewModel, CommentModel> replies;
        public ObservableModelCollection<CommentViewModel, CommentModel> Replies
        {
            get
            {
                if (replies.GetSource() != Model?.Replies)
                    replies.UpdateSource(Model?.Replies);
                return replies;
            }
        }
        public ObservableCollection<ReviewVoteModel> Votes => Model?.CommentReviewVotes;

        public IEnumerable<VoteEnum> VoteEnums => Enum.GetValues(typeof(VoteEnum)).Cast<VoteEnum>();

        public ICommand ReplyCmd { get; private set; }

        public ICommand AddVoteCmd { get; }
        public ICommand DeleteVoteCmd { get; }

        protected override CommentModel Template => new ReviewCommentModel(new UserModel("", "TestAuthor"))
        {
            Text = "Test comment, Kat0 please!\nWith line break, yeah!",
            Replies = new ObservableCollection<CommentModel>(new List<CommentModel>
            {
                new CommentModel(new UserModel("", "MemberTwo"))
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
            replies = new ObservableModelCollection<CommentViewModel, CommentModel>(x => x.ReplyTo = this);
            SetSource(Template);
            AddVoteCmd = new RelayCommand(o => AddVote(o as ReviewVoteModel), o => Model?.CommentReviewVotes != null);
            DeleteVoteCmd = new RelayCommand(o => DeleteVote(o as ReviewVoteModel), o => Model?.CommentReviewVotes != null && o is ReviewVoteModel);
        }

        public async override void SaveChanges()
        {
            IsLoading = true;
            try
            {
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

        public void AddVote()
        {
            AddVote(null);
        }

        public void AddVote(ReviewVoteModel vote)
        {
            if (Model == null)
                return;

            if (vote == null)
            {
                vote = new ReviewVoteModel();
            }

            Model.CommentReviewVotes.Add(vote);
        }

        public void DeleteVote(ReviewVoteModel vote)
        {
            if (Model == null)
                return;

            if (Model.CommentReviewVotes.Contains(vote))
                Model.CommentReviewVotes.Remove(vote);
        }

        public async Task<CommentModel> AddCommentAsync(CommentModel comment)
        {
            if (Model == null || comment == null)
                return null;

            try
            {
                IsLoading = true;
                comment.ReplyTo = Model;
                comment = await LeagueContext.AddModelAsync(comment);
                Model.Replies.Add(comment);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }

            return comment;
        }

        public async Task DeleteCommentAsync(CommentModel comment)
        {
            if (Model == null || comment == null || Model.Replies.Contains(comment) == false)
                return;

            try
            {
                IsLoading = true;
                await LeagueContext.DeleteModelAsync<CommentModel>(comment.CommentId.GetValueOrDefault());
                Model.Replies.Remove(comment);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<CommentModel> ReplyAsync(string text)
        {
            if (Model == null)
                return null;

            var author = LeagueContext.UserManager.CurrentUser;
            if (author == null)
                return null;

            var newComment = new CommentModel(author, Model) { Text = text };

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
