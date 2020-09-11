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

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaction logic for RegisterUserControl.xaml
    /// </summary>
    public partial class RegisterUserControl : UserControl, IModalContent
    {
        public CreateUserViewModel viewModel => DataContext as CreateUserViewModel;

        public string Header => "Register new User";

        public string SubmitText => "Register";

        public string CancelText => "Cancel";

        public RegisterUserControl()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && DataContext is CreateUserViewModel createUserVM)
            {
                createUserVM.SetPassword(passwordBox.Password);
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && DataContext is CreateUserViewModel createUserVM)
            {
                createUserVM.SetConfirmPassword(passwordBox.Password);
            }
        }

        public bool CanSubmit()
        {
            if (viewModel == null)
                return false;

            return viewModel.CanSubmit();
        }

        public async Task<bool> OnSubmitAsync()
        {
            return await viewModel.SubmitAsync();
        }

        public void OnCancel()
        {
            viewModel.Dispose();
        }
    }
}
