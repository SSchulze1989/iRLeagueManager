using iRLeagueManager.ViewModels;
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
using System.Windows.Shapes;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für UserLoginWindow.xaml
    /// </summary>
    public partial class UserLoginWindow : Window
    {
        private LoginViewModel LoginVM => DataContext as LoginViewModel;

        public UserLoginWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginVM == null)
                return;

            if (sender is Button button)
            {
                //LoginVM.SubmitButtonCommand.Execute(null);
                await LoginVM.SubmitAsync();
                if (LoginVM.IsLoggedIn)
                {
                    DialogResult = true;
                    Close();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                button.Command.Execute(button.CommandParameter);
                DialogResult = false;
                Close();
            }
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
    }
}
