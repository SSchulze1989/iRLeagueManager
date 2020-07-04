using iRLeagueManager.Data;
using iRLeagueManager.Enums;
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
                if (replys.GetSource() != Model?.Replys)
                    replys.UpdateSource(Model?.Replys);
                return replys;
            }
        }
        public ObservableCollection<VoteMemberAtFaultModel> Votes => Model?.Votes;

        public ICommand ReplyCmd { get; private set; }

        protected override CommentBase Template => new ReviewCommentModel(new LeagueMember(0, "Test", "Author"))
        {
            Text = "Test comment, Kat0 please!\nWith line break, yeah!",
            Replys = new ObservableCollection<CommentBase>(new List<CommentBase>
            {
                new CommentBase(new LeagueMember(0, "Member", "Two"))
                {
                    Text = "This is a reply!"
                }
            }),
            Votes = new ObservableCollection<VoteMemberAtFaultModel>(new List<VoteMemberAtFaultModel>()
            {
                new VoteMemberAtFaultModel() { Vote = VoteEnum.Kat1, MemberAtFault = new LeagueMember(0, "Bad", "Driver") }
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

            var author = await LeagueContext.GetModelAsync<LeagueMember>(LeagueContext.CurrentUser.MemberId.GetValueOrDefault());
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
