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
using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel mainViewModel;

        private SettingsPageViewModel SettingsPageViewModel { get; set; }
        private SchedulerViewModel SchedulerViewModel { get; set; }// = new SchedulerViewModel();
        private CalendarViewModel CalendarViewModel { get; set; }// = new CalendarViewModel();
        private ResultsPageViewModel ResultsPageViewModel { get; set; }// = new ResultsPageViewModel();

        public MainWindow()
        {
            InitializeComponent();
            mainViewModel = DataContext as MainWindowViewModel;
            Load();
        }

        public void Load()
        {
            mainViewModel.Load();
        }

        private void SchedulesButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainViewModel.CurrentSeason?.Schedules != null)
            {
                var vm = (MainContent.Content?.GetType().Equals(typeof(SchedulerViewModel))).GetValueOrDefault() ? MainContent.Content as SchedulerViewModel : SchedulerViewModel;
                if (vm == null)
                    SchedulerViewModel = vm = new SchedulerViewModel();
                MainContent.Content = vm;
                _ = vm.Load(mainViewModel.CurrentSeason.Model);
            }
            //GC.Collect();
        }

        private void RaceCalendarButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainViewModel.CurrentSeason?.Schedules != null)
            {
                var vm = (MainContent.Content?.GetType().Equals(typeof(CalendarViewModel))).GetValueOrDefault() ? MainContent.Content as CalendarViewModel : CalendarViewModel;
                if (vm == null)
                    CalendarViewModel = vm = new CalendarViewModel();
                MainContent.Content = vm;
                _ = vm.Load(mainViewModel.CurrentSeason.Model);
            }
        }

        private void StandingsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResultsButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainViewModel.CurrentSeason?.Schedules != null)
            {
                var vm = (MainContent.Content?.GetType().Equals(typeof(ResultsPageViewModel))).GetValueOrDefault() ? MainContent.Content as ResultsPageViewModel : ResultsPageViewModel;
                if (vm == null)
                    ResultsPageViewModel = vm = new ResultsPageViewModel();
                MainContent.Content = vm;
                _ = vm.Load(mainViewModel.CurrentSeason.Model);

                //var schedules = mainViewModel.CurrentSeason.Schedules;
                //if (schedules.Count > 0)
                //{
                //    var schedule = new ScheduleViewModel();
                //    await schedule.Load(schedules.First().ScheduleId.GetValueOrDefault());
                //    var session = schedule.Sessions.OrderBy(x => x.Date).LastOrDefault();
                //    var scoring = new ScoringViewModel();
                //    await scoring.Load(1);
                    
                //    await vm.Load(session.SessionId.GetValueOrDefault(), scoring.ScoringId.GetValueOrDefault());
                //}
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainViewModel.CurrentSeason != null)
            {
                if (SettingsPageViewModel == null)
                    SettingsPageViewModel = new SettingsPageViewModel();

                var vm = SettingsPageViewModel;

                MainContent.Content = vm;
                _ = vm.Load(mainViewModel.CurrentSeason.Model);
            }
        }
    }
}
