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

using iRLeagueManager.ViewModels;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für SchedulerControl.xaml
    /// </summary>
    public partial class SchedulerControl : UserControl
    {
        public SchedulerControl()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var editWindow = new ModalOkCancelWindow();
                editWindow.Width = 700;
                editWindow.Height = 650;
                var content = new SessionEditControl();
                var Schedule = button.DataContext as ScheduleViewModel;

                if (content.DataContext is SessionViewModel editVM && Schedule != null)
                {
                    editVM.Schedule = Schedule;

                    editWindow.ModalContent.Content = content;
                    if (editWindow.ShowDialog() == true)
                    {
                        Schedule.AddSession(editVM.Model);
                    }
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag != null)
            {
                var editWindow = new ModalOkCancelWindow();
                editWindow.Width = 700;
                editWindow.Height = 650;
                var content = new SessionEditControl();

                editWindow.Title = "Edit Session";

                if (content.DataContext is SessionViewModel editVM && button.Tag is SessionViewModel sessionVM)
                {
                    if (sessionVM.SessionType == Enums.SessionType.Race)
                    {
                        editVM.UpdateSource(Models.Sessions.RaceSessionModel.GetTemplate());
                    }
                    editVM.Model.CopyFrom(sessionVM.Model);
                    editVM.Schedule = sessionVM.Schedule;
                    
                    editWindow.ModalContent.Content = content;
                    if (editWindow.ShowDialog() == true)
                    {
                        sessionVM.Model.CopyFrom(editVM.Model);
                        sessionVM.SaveChanges();
                    }
                }
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

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {

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

                editWindow.ModalContent.Content = content;
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
    }
}
