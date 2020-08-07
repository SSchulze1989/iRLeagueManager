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
        private LoginViewModel ViewModel => DataContext as LoginViewModel;

        public UserLoginWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
                return;

            if (sender is Button button)
            {
                try
                {
                    //LoginVM.SubmitButtonCommand.Execute(null);
                    await ViewModel.SubmitAsync();
                    if (ViewModel.IsLoggedIn)
                    {
                        DialogResult = true;
                        Close();
                    }
                }
                catch
                {
                    ViewModel.StatusMessage = "Login failed - could not connect to Server.";
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink link && ViewModel != null)
            {
                var createWindow = new ModalOkCancelWindow();
                createWindow.Height = 450;
                createWindow.Width = 400;
                createWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                createWindow.Title = "Register new User";
                var content = new RegisterUserControl();

                if (content.DataContext is CreateUserViewModel createUserVM)
                {
                    createWindow.ModalContent.Content = content;

                    if (createWindow.ShowDialog() == true)
                    {
                        ViewModel.UserName = createUserVM.UserName;
                        ViewModel.SetPassword(null);
                        PasswordTextBox.Clear();
                    }
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ChangedButton == MouseButton.Left && Mouse.LeftButton == MouseButtonState.Pressed)
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
