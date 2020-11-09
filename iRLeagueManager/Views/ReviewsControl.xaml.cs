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
using iRLeagueManager.Extensions;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für ReviewsControl.xaml
    /// </summary>
    public partial class ReviewsControl : UserControl
    {
        public ReviewsPageViewModel ViewModel => DataContext as ReviewsPageViewModel;
        private ModalOkCancelControl EditPanel { get; } = new ModalOkCancelControl();

        public ReviewsControl()
        {
            InitializeComponent();
            MainGrid.Children.Add(EditPanel);
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is IncidentReviewViewModel reviewVM)
            {
                //var editWindow = new ModalOkCancelWindow();
                var editWindow = EditPanel;
                //editWindow.Width = 700;
                //editWindow.Height = 700;
                var content = new ReviewEditControl
                {
                    Header = "Edit Review"
                };

                if (content.DataContext is IncidentReviewViewModel editVM)
                {
                    editVM.Model.CopyFrom(reviewVM.Model);
                    await editVM.Refresh();

                    editWindow.ModalContent = content;
                    if (editWindow.ShowDialog() == true)
                    {
                        reviewVM.Model.CopyFrom(editVM.Model);
                        await reviewVM.SaveChanges();
                    }
                }
            }
        }

        private async void CommentAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                //var editWindow = new ModalOkCancelWindow();
                //editWindow.Width = 500;
                //editWindow.Height = 600;
                var editWindow = EditPanel;
                var content = new ReviewCommentEditControl
                {
                    Header = "Add new Comment",
                    SubmitText = "Add"
                };

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
                        editVM.UpdateSource(new ReviewCommentModel(reviewVM.CurrentUserModel, reviewVM.Model));
                        //editVM.Model.CommentReviewVotes = new ObservableCollection<ReviewVoteModel>(reviewComment.Model.CommentReviewVotes.ToList());
                        editVM.Review = reviewVM;
                        await editVM.Refresh();

                        editWindow.ModalContent = content;
                        if (editWindow.ShowDialog() == true)
                        {
                            await reviewVM.AddCommentAsync(editVM.Model);
                        }
                    }
                }
            }
        }

        private async void CommentEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is ReviewCommentViewModel reviewComment)
                {
                    //var editWindow = new ModalOkCancelWindow();
                    //editWindow.Width = 500;
                    //editWindow.Height = 600;
                    var editWindow = EditPanel;
                    var content = new ReviewCommentEditControl();

                    if (content.DataContext is ReviewCommentViewModel editVM)
                    {
                        editVM.UpdateSource(new ReviewCommentModel());
                        editVM.Model.CopyFrom(reviewComment.Model);
                        //editVM.Model.CommentReviewVotes = new ObservableCollection<ReviewVoteModel>(reviewComment.Model.CommentReviewVotes.ToList());
                        editVM.Review = reviewComment.Review;
                        await editVM.Refresh();

                        editWindow.ModalContent = content;
                        if (editWindow.ShowDialog() == true)
                        {
                            reviewComment.Model.CopyFrom(editVM.Model);
                            await reviewComment.SaveChanges();
                        }
                    }
                }
                else if (button.DataContext is CommentViewModel comment)
                {
                    //var editWindow = new ModalOkCancelWindow();
                    //editWindow.Width = 500;
                    //editWindow.Height = 280;
                    var editWindow = EditPanel;
                    var content = new CommentEditControl();

                    if (content.DataContext is CommentViewModel editVM)
                    {
                        editVM.Model = new CommentModel();
                        editVM.Model.CopyFrom(comment.Model);

                        editWindow.ModalContent = content;
                        if (editWindow.ShowDialog() == true)
                        {
                            comment.Model.CopyFrom(editVM.Model);
                            await comment.SaveChanges();
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
                    //var editWindow = new ModalOkCancelWindow();
                    //editWindow.Width = 500;
                    //editWindow.Height = 280;
                    var editWindow = EditPanel;
                    var content = new CommentEditControl();

                    content.Header = "Reply to Comment";
                    content.SubmitText = "Reply";

                    if (content.DataContext is CommentViewModel editVM)
                    {
                        editVM.Model = new CommentModel(ViewModel.CurrentUserModel);

                        editWindow.ModalContent = content;
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
                if (MessageBox.Show("Would you really like to delete this Comement?\nThis action can not be undone!", "Delete Comment", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    return;
                }

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
                    await editVM.Refresh();

                    editWindow.ModalContent = content;
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

        private async void ReviewDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is IncidentReviewViewModel reviewViewModel)
            {
                if (MessageBox.Show("Would you really like to delete this Review?\nThis action can not be undone!", "Delete Incident Review", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await ViewModel.RemoveReviewAsync(reviewViewModel.Model);
                }
                e.Handled = true;
            }
        }

        private void ReviewsNavBarControl_SelectedReviewChanged(object sender, SelectionChangedEventArgs e)
        {
            var oldSelection = e.RemovedItems.Count > 0 ? e.RemovedItems[0] as IncidentReviewViewModel : null;
            var newSelection = e.AddedItems.Count > 0 ? e.AddedItems[0] as IncidentReviewViewModel : null;

            if (oldSelection != null)
            {
                oldSelection.IsExpanded = false;
            }
            if (newSelection != null)
            {
                newSelection.IsExpanded = true;
                ReviewsItemsControl.ScrollIntoView(newSelection);
            }
        }

        private async void EditResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is IncidentReviewViewModel reviewVM)
            {
                //var editWindow = new ModalOkCancelWindow();
                var editWindow = EditPanel;
                //editWindow.Width = 700;
                //editWindow.Height = 700;
                var content = new ReviewResultControl();

                if (content.DataContext is IncidentReviewViewModel editVM)
                {
                    editVM.Model.CopyFrom(reviewVM.Model);
                    await editVM.Refresh();

                    editWindow.ModalContent = content;
                    if (editWindow.ShowDialog() == true)
                    {
                        reviewVM.Model.CopyFrom(editVM.Model);
                        await reviewVM.SaveChanges();
                    }
                }
            }
        }
    }
}
