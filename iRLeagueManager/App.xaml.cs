using iRLeagueManager.ViewModels;
using iRLeagueManager.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace iRLeagueManager
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var dialog = new UserLoginWindow();
            var viewModel = new LoginViewModel();
#if DEBUG
            viewModel.UserName = "Administrator";
            viewModel.SetPassword("administrator");
#endif
            dialog.DataContext = viewModel;
            viewModel.Load();


            if (dialog.ShowDialog() == true)
            {
                //var mainWindow = new MainWindow();
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                //Current.MainWindow = mainWindow;
                //MainWindow.Show();
            }
            else
            {
                Current.Shutdown(-1);
            }
        }
    }
}
