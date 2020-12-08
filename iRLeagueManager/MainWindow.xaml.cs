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
using System.Reflection;
using System.Net.NetworkInformation;
using System.Diagnostics;
using iRLeagueManager.Views;
using iRLeagueManager.Logging;

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
        private StandingsPageViewModel StandingsPageViewModel { get; set; }
        private ReviewsPageViewModel ReviewsPageViewModel { get; set; }
        private TeamsPageViewModel TeamsPageViewModel { get; set; }

        private ModalOkCancelControl EditPanel { get; }

        public MainWindow()
        {
            InitializeComponent();
            EditPanel = new ModalOkCancelControl();
            MainGrid.Children.Add(EditPanel);

            mainViewModel = DataContext as MainWindowViewModel;

            if (mainViewModel != null)
            {
                mainViewModel.SeasonChanged += async (sender, eventArgs) =>
                {
                    if (mainViewModel.SeasonList?.Count > 0)
                    {
                        if (MainContent.Content is ViewModelBase contentViewModel)
                        {
                            if (contentViewModel is ISeasonPageViewModel seasonPageViewModel)
                            {
                                await seasonPageViewModel.Load(mainViewModel.SelectedSeason);
                            }
                            else if (contentViewModel is IPageViewModel pageViewModel)
                            {
                                await pageViewModel.Load();
                            }
                            else
                            {
                                await contentViewModel.Refresh();
                            }
                        }
                    }
                    else
                        await mainViewModel.Refresh();
                };
            }
            var assembly = Assembly.GetExecutingAssembly();
            Title = "iRLeagueManager v" + assembly.GetName().Version.ToString(3);
            Load();
        }

        public async Task Load()
        {
            await mainViewModel.Load();
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
            if (mainViewModel.CurrentSeason?.Schedules != null)
            {
                if (StandingsPageViewModel == null)
                    StandingsPageViewModel = new StandingsPageViewModel();
                var vm = StandingsPageViewModel;
                MainContent.Content = vm;
                _ = vm.Load(mainViewModel.CurrentSeason.Model);
            }
        }

        private async void ResultsButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainViewModel.CurrentSeason?.Schedules != null)
            {
                var vm = (MainContent.Content?.GetType().Equals(typeof(ResultsPageViewModel))).GetValueOrDefault() ? MainContent.Content as ResultsPageViewModel : ResultsPageViewModel;
                if (vm == null)
                    ResultsPageViewModel = vm = new ResultsPageViewModel();
                MainContent.Content = vm;
                await vm.Load(mainViewModel.CurrentSeason.Model);

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

        private async void ReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainViewModel.CurrentSeason?.Schedules != null)
            {
                //var vm = (MainContent.Content?.GetType().Equals(typeof(ReviewsPageViewModel))).GetValueOrDefault() ? MainContent.Content as ReviewsPageViewModel : ReviewsPageViewModel;
                var vm = (MainContent.Content is ReviewsPageViewModel reviewsPageViewModel) ? reviewsPageViewModel : ReviewsPageViewModel;
                if (vm == null)
                    ReviewsPageViewModel = vm = new ReviewsPageViewModel();
                MainContent.Content = vm;
                await vm.Load(mainViewModel.CurrentSeason.Model);
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

        private async void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            if (mainViewModel != null)
            {
                if (mainViewModel.SeasonList?.Count > 0)
                {
                    if (MainContent.Content is ViewModelBase contentViewModel)
                    {
                        await contentViewModel.Refresh();
                    }
                }
                else
                    await mainViewModel.Refresh();
            }
        }

        private async void TeamsButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainViewModel.CurrentSeason?.Schedules != null)
            {
                var vm = (MainContent.Content?.GetType().Equals(typeof(TeamsPageViewModel))).GetValueOrDefault() ? MainContent.Content as TeamsPageViewModel : TeamsPageViewModel;
                if (vm == null)
                    TeamsPageViewModel = vm = new TeamsPageViewModel();
                MainContent.Content = vm;
                await vm.Load();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void GitHub_Click(object sender, RoutedEventArgs e)
        {
            var eventArgs = new RequestNavigateEventArgs(new Uri("https://github.com/SSchulze1989/iRLeagueManager"), "https://github.com/SSchulze1989/iRLeagueManager");
            Hyperlink_RequestNavigate(sender, eventArgs);
            e.Handled = true;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var content = new PasswordEditControl();
            EditPanel.ModalContent = content;

            if (EditPanel.ShowDialog() == true)
            {
                MessageBox.Show("Password succesfully changed!", "Passwor changed", MessageBoxButton.OK);
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            var content = new UserEditControl();
            EditPanel.ModalContent = content;

            if (EditPanel.ShowDialog() == true)
            {
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                if (row.Item is ExceptionLogMessage msg)
                {
                    MessageBox.Show(msg.Exception.ToString(), $"Error - {msg.Message}", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
