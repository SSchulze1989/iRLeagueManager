using iRLeagueManager.ViewModels;
using iRLeagueManager.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Globalization;

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

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.GetCultureInfo("de-DE").IetfLanguageTag)));

            var dialog = new UserLoginWindow();
            var viewModel = new LoginViewModel();
#if DEBUG
            viewModel.UserName = "Administrator";
            viewModel.SetPassword("administrator");
#endif

            dialog.DataContext = viewModel;
            dialog.WindowStyle = WindowStyle.None;
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
