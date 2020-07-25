using iRLeagueManager.Models.Reviews;
using iRLeagueManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für ReviewsControl.xaml
    /// </summary>
    public partial class ReviewsControl : UserControl
    {
        public ReviewsPageViewModel ViewModel => DataContext as ReviewsPageViewModel;

        public ReviewsControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is IncidentReviewViewModel incidentReview)
            {
                incidentReview.Hold();
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is IncidentReviewViewModel reviewVM)
            {
                var editWindow = new ModalOkCancelWindow();
                editWindow.Width = 700;
                editWindow.Height = 700;
                var content = new ReviewEditControl();

                editWindow.Title = "Edit Review";

                if (content.DataContext is IncidentReviewViewModel editVM)
                {
                    editVM.Model.CopyFrom(reviewVM.Model);
                    await editVM.LoadMemberListAsync();

                    editWindow.ModalContent.Content = content;
                    if (editWindow.ShowDialog() == true)
                    {
                        reviewVM.Model.CopyFrom(editVM.Model);
                        reviewVM.SaveChanges();
                    }
                }
            }
        }

        private async void CommentAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var editWindow = new ModalOkCancelWindow();
                editWindow.Width = 500;
                editWindow.Height = 600;
                var content = new ReviewCommentEditControl();

                editWindow.Title = "New Comment";
                if (content.DataContext is ReviewCommentViewModel editVM && ViewModel.SelectedReview != null)
                {
                    var reviewVM = ViewModel.SelectedReview;
                    editVM.UpdateSource(new ReviewCommentModel(reviewVM.CurrentUser, reviewVM.Model));
                    //editVM.Model.CommentReviewVotes = new ObservableCollection<ReviewVoteModel>(reviewComment.Model.CommentReviewVotes.ToList());
                    editVM.Review = ViewModel.SelectedReview;
                    editVM.Refresh(null);

                    editWindow.ModalContent.Content = content;
                    if (editWindow.ShowDialog() == true)
                    {
                        await reviewVM.AddCommentAsync(editVM.Model);
                    }
                }
            }
        }

        private void CommentEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag is ReviewCommentViewModel reviewComment)
                {
                    var editWindow = new ModalOkCancelWindow();
                    editWindow.Width = 500;
                    editWindow.Height = 600;
                    var content = new ReviewCommentEditControl();

                    editWindow.Title = "Edit Comment";

                    if (content.DataContext is ReviewCommentViewModel editVM)
                    {
                        editVM.UpdateSource(new ReviewCommentModel());
                        editVM.Model.CopyFrom(reviewComment.Model);
                        //editVM.Model.CommentReviewVotes = new ObservableCollection<ReviewVoteModel>(reviewComment.Model.CommentReviewVotes.ToList());
                        editVM.Review = ViewModel.SelectedReview;
                        editVM.Refresh(null);

                        editWindow.ModalContent.Content = content;
                        if (editWindow.ShowDialog() == true)
                        {
                            reviewComment.Model.CopyFrom(editVM.Model);
                            reviewComment.SaveChanges();
                        }
                    }
                }
                else if (button.Tag is CommentViewModel comment)
                {
                    var editWindow = new ModalOkCancelWindow();
                    editWindow.Width = 500;
                    editWindow.Height = 280;
                    var content = new CommentEditControl();

                    editWindow.Title = "Edit Comment";

                    if (content.DataContext is CommentViewModel editVM)
                    {
                        editVM.Model = new CommentModel();
                        editVM.Model.CopyFrom(comment.Model);

                        editWindow.ModalContent.Content = content;
                        if (editWindow.ShowDialog() == true)
                        {
                            comment.Model.CopyFrom(editVM.Model);
                            comment.SaveChanges();
                        }
                    }
                }
            }
        }

        private async void CommentReplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag is ReviewCommentViewModel reviewCommentVM)
                {
                    var editWindow = new ModalOkCancelWindow();
                    editWindow.Width = 500;
                    editWindow.Height = 280;
                    var content = new CommentEditControl();

                    editWindow.Title = "Edit Comment";

                    if (content.DataContext is CommentViewModel editVM)
                    {
                        editVM.Model = new CommentModel(ViewModel.CurrentUser);

                        editWindow.ModalContent.Content = content;
                        if (editWindow.ShowDialog() == true)
                        {
                            await reviewCommentVM.AddCommentAsync(editVM.Model);
                        }
                    }
                }
            }
        }

        private async void CommentDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag is ReviewCommentViewModel reviewCommentVM)
                {
                    var selectedReview = ViewModel.SelectedReview;

                    if (selectedReview != null && selectedReview.Comments.Contains(reviewCommentVM))
                    {
                        await selectedReview.DeleteCommentAsync(reviewCommentVM.Model);
                    }
                }
                else if (button.Tag is CommentViewModel commentVM)
                {
                    var replyTo = commentVM.ReplyTo;

                    if (replyTo != null && replyTo.Replies.Contains(commentVM))
                    {
                        await replyTo.DeleteCommentAsync(commentVM.Model);
                    }
                }
            }
        }
    }
}
