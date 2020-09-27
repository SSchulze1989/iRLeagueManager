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
using CredentialManagement;
using AutoUpdaterDotNET;

namespace iRLeagueManager
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AutoUpdater.UpdateMode = Mode.ForcedDownload;
            AutoUpdater.Start("http://144.91.113.195/updates/update.xml");

            if (Current != null)
            {
                Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            }

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.GetCultureInfo("de-DE").IetfLanguageTag)));

            var dialog = new UserLoginWindow();
            //            var viewModel = new LoginViewModel();

            //dialog.DataContext = viewModel;
            dialog.WindowStyle = WindowStyle.None;

            if (dialog.ShowDialog() == true)
            {
                var leagueSelectDialog = new ModalOkCancelWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Height = 200,
                    Width = 400,
                    ShowHeader = false
                };
                leagueSelectDialog.ModalContent = new SelectLeagueControl();
                if (leagueSelectDialog.ShowDialog() == true)
                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                else
                    Current.Shutdown(-1);
            }
            else
            {
                if (Current != null)
                {
                    Current.Shutdown(-1);
                }
            }
        }
    }
}
