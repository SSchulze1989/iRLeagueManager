using iRLeagueManager.Controls;
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
                //var editWindow = new ModalOkCancelWindow();
                var editWindow = EditPanel;
                //editWindow.Width = 700;
                //editWindow.Height = 700;
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

                IncidentReviewViewModel reviewVM = null;
                if (content.DataContext is ReviewCommentViewModel editVM)
                {
                    if (ViewModel.SelectedReview != null)
                    {
                        reviewVM = ViewModel.SelectedReview;
                    }
                    else if (button.Tag is IncidentReviewViewModel currentReviewViewModel)
                    {
                        reviewVM = currentReviewViewModel;
                    }

                    if (reviewVM != null)
                    {
                        editVM.UpdateSource(new ReviewCommentModel(reviewVM.CurrentUser, reviewVM.Model));
                        //editVM.Model.CommentReviewVotes = new ObservableCollection<ReviewVoteModel>(reviewComment.Model.CommentReviewVotes.ToList());
                        editVM.Review = reviewVM;
                        editVM.Refresh(null);

                        editWindow.ModalContent.Content = content;
                        if (editWindow.ShowDialog() == true)
                        {
                            await reviewVM.AddCommentAsync(editVM.Model);
                        }
                    }
                }
            }
        }

        private void CommentEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is ReviewCommentViewModel reviewComment)
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
                        editVM.Review = reviewComment.Review;
                        editVM.Refresh(null);

                        editWindow.ModalContent.Content = content;
                        if (editWindow.ShowDialog() == true)
                        {
                            reviewComment.Model.CopyFrom(editVM.Model);
                            reviewComment.SaveChanges();
                        }
                    }
                }
                else if (button.DataContext is CommentViewModel comment)
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
                if (button.DataContext is ReviewCommentViewModel reviewCommentVM)
                {
                    var review = reviewCommentVM.Review;

                    if (review != null && review.Comments.Contains(reviewCommentVM))
                    {
                        await review.DeleteCommentAsync(reviewCommentVM.Model);
                    }
                }
                else if (button.DataContext is CommentViewModel commentVM)
                {
                    var replyTo = commentVM.ReplyTo;

                    if (replyTo != null && replyTo.Replies.Contains(commentVM))
                    {
                        await replyTo.DeleteCommentAsync(commentVM.Model);
                    }
                }
            }
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender is Button || sender is Hyperlink)  && ViewModel != null)
            {
                var editWindow = EditPanel;
                //editWindow.Width = 700;
                //editWindow.Height = 700;
                var content = new ReviewEditControl();

                editWindow.Title = "Add new Review";

                if (content.DataContext is IncidentReviewViewModel editVM)
                {
                    editVM.Model = ViewModel.CreateReviewModel();
                    await editVM.LoadMemberListAsync();

                    editWindow.ModalContent.Content = content;
                    if (editWindow.ShowDialog() == true)
                    {
                        await ViewModel.AddReviewAsync(editVM.Model);
                    }
                }
            }
            e.Handled = true;
        }

        private void TreeView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = e.Source;

            if (sender is TreeView treeView)
            {
                DependencyObject parent = treeView;
                ScrollViewer treeViewScroll = null;
                while(parent != null && treeViewScroll == null)
                {
                    parent = VisualTreeHelper.GetParent(parent);
                    treeViewScroll = parent as ScrollViewer;
                }

                if (treeViewScroll != null)
                {
                    ScrollViewer_PreviewMouseWheel(treeViewScroll, eventArg);
                    if (eventArg.Handled == false)
                    {
                        treeViewScroll.RaiseEvent(eventArg);
                    }
                    e.Handled = true;
                }
            }
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = e.Source;

            ScrollViewer scv = verticalContentScroll;
            scv.RaiseEvent(eventArg);
            e.Handled = true;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = e.Source;

            if (sender is ScrollViewer scrollViewer)
            {
                var isScrolledToTop = scrollViewer.VerticalOffset == 0;
                var isScrolledToBottom = scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight;

                if ((isScrolledToTop && e.Delta > 0) || (isScrolledToBottom && e.Delta < 0))
                {
                    ScrollViewer scv = verticalContentScroll;
                    scv.RaiseEvent(eventArg);
                    e.Handled = true;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.ClickCount >= 2)
                {
                    var button = element.FindName("expandButton") as IconToggleButton;
                    if (button != null)
                    {
                        button.IsChecked = !button.IsChecked;
                    }
                }
            }
        }
    }
}
