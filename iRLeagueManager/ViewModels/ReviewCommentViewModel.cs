// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Data;

namespace iRLeagueManager.ViewModels
{
    public class ReviewCommentViewModel : CommentViewModel, IContainerModelBase<ReviewCommentModel>
    {
        public new ReviewCommentModel Model => base.Model as ReviewCommentModel;

        private IncidentReviewViewModel review;
        public IncidentReviewViewModel Review { get => review; set => SetValue(ref review, value); }
        
        public ObservableViewModelCollection<CommentViewModel, CommentModel> replies;
        public ObservableViewModelCollection<CommentViewModel, CommentModel> Replies
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

        private ICollectionView voteCategories;
        public ICollectionView VoteCategories { get => voteCategories; set => SetValue(ref voteCategories, value); }

        public ICommand ReplyCmd { get; private set; }

        public ICommand AddVoteCmd { get; }
        public ICommand DeleteVoteCmd { get; }

        protected override CommentModel Template => new ReviewCommentModel(new UserModel("", "TestAuthor"))
        {
            Text = "Test comment, Kat0 please!\nWith line break, yeah!"
        };

        public ReviewCommentViewModel()
        {
            ReplyCmd = new RelayCommand(o => { }, o => false);
            replies = new ObservableViewModelCollection<CommentViewModel, CommentModel>(x => x.ReplyTo = this);
            SetSource(Template);
            AddVoteCmd = new RelayCommand(o => AddVote(o as ReviewVoteModel), o => Model?.CommentReviewVotes != null);
            DeleteVoteCmd = new RelayCommand(o => DeleteVote(o as ReviewVoteModel), o => Model?.CommentReviewVotes != null && o is ReviewVoteModel);
        }

        public ReviewCommentViewModel(ReviewCommentModel comment) : base(comment) { }

        public async override Task SaveChanges()
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
            Review?.OnCommentsCollectionChanged(null, new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }

        public async override Task Refresh()
        {
            try
            {
                IsLoading = true;
                var votesCategorieCollection = await LeagueContext.GetModelsAsync<VoteCategoryModel>();
                SetVoteCategoriesView(votesCategorieCollection);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
            await base.Refresh();
        }

        private void SetVoteCategoriesView(object source)
        {
            VoteCategories = CollectionViewSource.GetDefaultView(source);
            VoteCategories.SortDescriptions.Add(new SortDescription(nameof(VoteCategoryModel.Index), ListSortDirection.Ascending));
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            switch(propertyName)
            {
                case nameof(Model.CommentReviewVotes):
                    base.OnPropertyChanged(nameof(Votes));
                    break;
            }

            base.OnPropertyChanged(propertyName);
        }

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
