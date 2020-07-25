using iRLeagueManager.Interfaces;
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
    /// Interaktionslogik für UserLoginControl.xaml
    /// </summary>
    public partial class UserLoginControl : UserControl
    {
        public LoginViewModel ViewModel => DataContext as LoginViewModel;

        public UserLoginControl()
        {
            InitializeComponent();
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                var password = passwordBox.Password;
                if (DataContext is IHasPassword hasPassword)
                {
                    hasPassword.SetPassword(password);
                }
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink link && ViewModel != null)
            {
                var createWindow = new ModalOkCancelWindow();
                createWindow.Height = 300;
                createWindow.Width = 300;
                createWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                var content = new RegisterUserControl();

                if (content.DataContext is CreateUserViewModel createUserVM)
                {
                    createWindow.Content = content;

                    if (createWindow.ShowDialog() == true)
                    {
                        ViewModel.UserName = createUserVM.UserName;
                        ViewModel.SetPassword(null);
                        PasswordTextBox.Clear();
                    }
                }
            }
        }
    }
}
