﻿using iRLeagueManager.ViewModels;
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
            //            var viewModel = new LoginViewModel();

            //dialog.DataContext = viewModel;
            dialog.WindowStyle = WindowStyle.None;

            if (dialog.ShowDialog() == true)
            {
                var leagueSelectDialog = new ModalOkCancelWindow();
                leagueSelectDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                leagueSelectDialog.Height = 200;
                leagueSelectDialog.Width = 400;
                leagueSelectDialog.ModalContent.Content = new SelectLeagueControl();
                if (leagueSelectDialog.ShowDialog() == true)
                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                else
                    Current.Shutdown(-1);
            }
            else
            {
                Current.Shutdown(-1);
            }
        }
    }
}
