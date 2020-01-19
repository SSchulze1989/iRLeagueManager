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
                var content = new SessionEditControl();
                var Schedule = button.DataContext as ScheduleViewModel;

                if (content.DataContext is SessionViewModel editVM && Schedule != null)
                {
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
                var content = new SessionEditControl();
                
                if (content.DataContext is SessionViewModel editVM && button.Tag is SessionViewModel sessionVM)
                {
                    if (sessionVM.SessionType == Enums.SessionType.Race)
                    {
                        editVM.UpdateSource(Models.Sessions.RaceSessionModel.GetTemplate());
                    }
                    editVM.Model.CopyFrom(sessionVM.Model);
                    
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
                    schedulerVM
                }
            }
        }
    }
}
