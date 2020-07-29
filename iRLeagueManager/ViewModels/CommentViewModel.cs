﻿using iRLeagueManager.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.ViewModels
{
    public class CommentViewModel : LeagueContainerModel<CommentModel>
    {
        public long CommentId => (Model?.CommentId).GetValueOrDefault();

        //private IncidentReviewViewModel review;
        //public IncidentReviewViewModel Review { get => review; set => SetValue(ref review, value); }
        //public LeagueMember Author => Model?.Author;

        private UserViewModel author = new UserViewModel();
        public UserViewModel Author
        {
            get
            {
                if (author.UserId != Model?.Author?.UserId)
                {
                    if (author.UpdateSource(Model.Author))
                        OnPropertyChanged();
                }
                return author;
            }
        }


        public string AuthorName => Model?.AuthorName;
        public string Text { get => Model?.Text; set => Model.Text = value; }
        public DateTime Date => (Model?.Date).GetValueOrDefault();
        protected override CommentModel Template => new ReviewCommentModel(new UserModel("", "MemberTwo"))
        {
            Text = "This is a reply!\nAlso with a line break!"
        };
        //public bool IsUserAuthor => (LeagueContext.CurrentUser?.MemberId).GetValueOrDefault() == Author.MemberId.GetValueOrDefault();
        public bool IsUserAuthor => LeagueContext?.UserManager?.CurrentUser?.UserId == Author?.UserId || LeagueContext?.UserManager?.CurrentUser?.UserName == "Administrator";

        private ReviewCommentViewModel replyTo;
        public ReviewCommentViewModel ReplyTo { get => replyTo; set => SetValue(ref replyTo, value); }

        public ICommand EditCmd { get; private set; }

        public CommentViewModel() : this(null)
        {
            SetSource(Template);
        }
        
        public CommentViewModel(CommentModel source) : base(source) 
        {
            EditCmd = new RelayCommand(async o => await EditAsync(o as string), o => IsUserAuthor);
        }

        public async Task<bool> EditAsync(string editedText)
        {
            string oldText = Text;
            bool status = false;

            try
            {
                //if (LeagueContext.CurrentUser.MemberId != Author.MemberId)
                if (IsUserAuthor == false)
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