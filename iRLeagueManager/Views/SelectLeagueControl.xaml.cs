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
