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
            AppDomain.CurrentDomain.UnhandledException += (exceptionSender, eventArgs) =>
            {
                GlobalSettings.LogError(eventArgs.ExceptionObject as Exception);
            };

#if !(DEBUG)
            AutoUpdater.UpdateMode = Mode.ForcedDownload;
            AutoUpdater.Start("http://144.91.113.195/updates/update.xml");
#endif

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
