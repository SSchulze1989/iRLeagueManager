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
using System.Net.Http;

using iRLeagueManager.Data;
using iRLeagueManager.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für SelectLeagueControl.xaml
    /// </summary>
    public partial class SelectLeagueControl : UserControl, IModalContent, INotifyPropertyChanged
    {
        private bool isLoading;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(); } }

        public SelectLeagueControl()
        {
            InitializeComponent();
        }

        public string Header => "Select League";

        public string SubmitText => "Open";

        public string CancelText => "Cancel";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void OnLoad()
        {
            try
            {
                IsLoading = true;
                var leagueNames = await GlobalSettings.LeagueContext.LeagueDataProvider.GetLeagueNames();
                LeagueNameComboBox.ItemsSource = leagueNames;
                LeagueNameComboBox.SelectedItem = leagueNames.LastOrDefault();
            }
            catch
            {
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public bool CanSubmit()
        {
            return LeagueNameComboBox.Text != "";
        }

        public void OnCancel()
        {
        }

        public async Task<bool> OnSubmitAsync()
        {
            try
            {
                IsLoading = true;
                var leagueName = LeagueNameComboBox.Text;
                var exists = await GlobalSettings.LeagueContext.LeagueDataProvider.CheckLeagueExists(leagueName);
                GlobalSettings.LeagueContext.SetLeagueName(leagueName);
                return exists;
            }
            catch (Exception e)
            {
                if (e is UserNotAuthorizedException)
                {
                    StatusMessageTextBLock.Text = "You are not allowed to open this League";
                    return false;
                }
                else if (e is LeagueNotFoundException)
                {
                    StatusMessageTextBLock.Text = "This league name does not exist";
                    return false;
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
