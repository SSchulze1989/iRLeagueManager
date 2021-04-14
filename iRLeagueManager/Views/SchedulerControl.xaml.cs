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

using System;
using System.Collections.Generic;
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
using iRLeagueManager.Controls;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.ViewModels;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für SchedulerControl.xaml
    /// </summary>
    public partial class SchedulerControl : UserControl
    {
        private SchedulerViewModel ViewModel => DataContext as SchedulerViewModel;

        public SchedulerControl()
        {
            InitializeComponent();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var editWindow = new ModalOkCancelControl();
                try
                {
                    //editWindow.Width = 700;
                    //editWindow.Height = 650;
                    editWindow.Title = "Add New Session";
                    var content = new SessionEditControl();
                    var Schedule = button.DataContext as ScheduleViewModel;
                    MainGrid.Children.Add(editWindow);

                    if (content.DataContext is SessionViewModel editVM && Schedule != null)
                    {
                        editVM.Schedule = Schedule;

                        editWindow.ModalContent = content;
                        if (editWindow.ShowDialog() == true)
                        {
                            await Schedule.AddSessionAsync(editVM.Model);
                        }
                    }
                }
                finally
                {
                    MainGrid.Children.Remove(editWindow);
                }
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                await EditSessionDialog(button.Tag as SessionViewModel);
            }
        }

        private async Task EditSessionDialog(SessionViewModel sessionVM)
        {
            if (sessionVM == null)
            {
                return;
            }

            var editWindow = new ModalOkCancelControl();
            try
            {
                //editWindow.Width = 700;
                //editWindow.Height = 650;
                var content = new SessionEditControl();
                MainGrid.Children.Add(editWindow);

                editWindow.Title = "Edit Session";

                var editVM = content.DataContext as SessionViewModel;
                if (sessionVM.SessionType == Enums.SessionType.Race)
                {
                    editVM.UpdateSource(Models.Sessions.RaceSessionModel.GetTemplate());
                }
                editVM.Model.CopyFrom(sessionVM.Model);
                editVM.Schedule = sessionVM.Schedule;

                editWindow.ModalContent = content;
                if (editWindow.ShowDialog() == true)
                {
                    sessionVM.Model.CopyFrom(editVM.Model);
                    await sessionVM.SaveChanges();
                }
            }
            finally
            {
                MainGrid.Children.Remove(editWindow);
            }
        }

        private void ScheduleDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SchedulerViewModel schedulerVM && sender is Button button && button.Tag is ScheduleViewModel scheduleVM)
            {
                if (MessageBox.Show("Would you really like to delete Schedule: " + scheduleVM.Name + "?\nThis action can not be undone!", "Delete Schedule", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    
                }
            }
        }

        private async void DeleteResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag is SessionViewModel sessionViewModel)
                {
                    if (MessageBox.Show("Do you Really want to delete the Result set? This will remove all associated reviews and given penalties.", "Remove Result", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        await sessionViewModel.DeleteResultFile();
                    }
                }
            }
        }

        private void MoveSessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is SchedulerViewModel schedulerVM))
                return;

            if (sender is Button button && button.Tag != null)
            {
                var editWindow = new ModalOkCancelWindow();
                editWindow.Width = 300;
                editWindow.Height = 200;
                var content = new UserControl();

                var grid = new Grid();
                var textBlock = new TextBlock();
                textBlock.Inlines.Add("Dies ist ein Text");
                grid.Children.Add(textBlock);

                editWindow.Title = "Select Schedule";

                if (!(button.Tag is SessionViewModel sessionVM))
                    return;
                var currentScheduleVM = sessionVM.Schedule;

                var stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                var description = new TextBlock();
                description.Inlines.Add("Select target Schedule");
                stackPanel.Children.Add(description);

                var schedules = schedulerVM.Schedules.Where(x => x.ScheduleId != currentScheduleVM.ScheduleId);
                var comboBox = new ComboBox
                {
                    ItemsSource = schedules,
                    SelectedIndex = 0,
                    DisplayMemberPath = "Name"
                };
                stackPanel.Children.Add(comboBox);

                content.Content = stackPanel;

                editWindow.ModalContent = content;
                //editWindow.Content = content;
                if (editWindow.ShowDialog() == true)
                {
                    var targetSchedule = comboBox.SelectedItem as ScheduleViewModel;
                    schedulerVM.MoveSessionToSchedule(sessionVM.Model, currentScheduleVM.Model, targetSchedule.Model);
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

        private void SchedulePanel_MouseDown(object sender, MouseButtonEventArgs e)
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

        private async void UploadResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is SessionViewModel session)
            {
                try
                {
                    session.SelectResultNameDialog += OpenSelectDialog;
                    session.SelectSubSessionDialog += OpenSelectDialog;
                    await session.UploadFile(session.Model);
                }
                finally
                {
                    session.SelectResultNameDialog -= OpenSelectDialog;
                    session.SelectSubSessionDialog -= OpenSelectDialog;
                }
            }
        }

        private T OpenSelectDialog<T>(SessionViewModel sender, string title, string message, IEnumerable<T> values) where T : class
        {
            var dialog = new ModalOkCancelControl();
            try
            {
                var content = new SelectionControl()
                {
                    Header = title,
                    Message = message,
                    Items = CollectionViewSource.GetDefaultView(values),
                    SubmitText = "Ok",
                    CancelText = "Cancel"
                };
                dialog.ModalContent = content;
                MainGrid.Children.Add(dialog);

                if (dialog.ShowDialog() == true)
                {
                    return (T)content.Items.CurrentItem;
                }
                return null;
            }
            finally
            {
                MainGrid.Children.Remove(dialog);
            }
        }

        private async void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.DataContext is SessionViewModel sessionVM)
            {
                e.Handled = true;
                await EditSessionDialog(sessionVM);
            }
        }
    }
}
