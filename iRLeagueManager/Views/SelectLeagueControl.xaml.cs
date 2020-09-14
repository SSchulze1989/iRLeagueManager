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

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für SelectLeagueControl.xaml
    /// </summary>
    public partial class SelectLeagueControl : UserControl, IModalContent
    {
        public SelectLeagueControl()
        {
            InitializeComponent();
        }

        public string Header => "Select League";

        public string SubmitText => "Open";

        public string CancelText => "Cancel";

        public bool IsLoading = false;

        public bool CanSubmit()
        {
            return LeagueNameTextBox.Text != "";
        }

        public void OnCancel()
        {
        }

        public async Task<bool> OnSubmitAsync()
        {
            try
            {
                IsLoading = true;
                var leagueName = LeagueNameTextBox.Text;
                var exists = await GlobalSettings.LeagueContext.LeagueDataProvider.CheckLeagueExists(leagueName);
                GlobalSettings.LeagueContext.LeagueName = leagueName;
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
