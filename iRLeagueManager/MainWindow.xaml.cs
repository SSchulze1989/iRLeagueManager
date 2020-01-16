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
                var vm = (MainContent.Content is SchedulerViewModel currentVm) ? currentVm : new SchedulerViewModel();
                MainContent.Content = vm;
                vm.Load(mainViewModel.CurrentSeason.Model);
            }
        }
    }
}
