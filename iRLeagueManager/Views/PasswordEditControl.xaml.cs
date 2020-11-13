using iRLeagueManager.ViewModels;
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

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für PasswordEditControl.xaml
    /// </summary>
    public partial class PasswordEditControl : UserControl, IModalContent
    {
        private ChangePasswordViewModel ViewModel => DataContext as ChangePasswordViewModel;

        public PasswordEditControl()
        {
            InitializeComponent();
        }

        public string Header => "Change Password";

        public string SubmitText => "Submit";

        public string CancelText => "Cancel";

        public bool IsLoading { get; set; }

        public bool CanSubmit()
        {
            return true;
        }

        public void OnCancel()
        {
            
        }

        public void OnLoad()
        {
            DataContext = new ChangePasswordViewModel();
            ViewModel.UpdateSource(GlobalSettings.LeagueContext.UserManager.CurrentUser);
        }

        public async Task<bool> OnSubmitAsync()
        {
            if (ViewModel == null)
            {
                return false;
            }

            var result = await ViewModel.SubmitAsync();
            if (result == true)
            {
                ViewModel.UpdateSource(null);
                OldPasswordBox.Password = "";
                NewPasswordBox.Password = "";
                ConfirmPasswordBox.Password = "";
                ViewModel.Dispose();
                DataContext = null;
                return true;
            }
            return false;
        }

        private void OldPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel?.SetOldPassword(((PasswordBox)sender).Password);
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel?.SetNewPassword(((PasswordBox)sender).Password);
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel?.SetConfirmPassword(((PasswordBox)sender).Password);
        }
    }
}
