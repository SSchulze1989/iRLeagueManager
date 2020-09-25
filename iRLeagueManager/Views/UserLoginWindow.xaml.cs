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
using System.Windows.Controls.Primitives;
using CredentialManagement;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für UserLoginWindow.xaml
    /// </summary>
    public partial class UserLoginWindow : Window
    {
        public const string credentialTarget = "iRLeagueManager_Desktop";
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
                        if (ViewModel.RememberMe)
                        {
                            var credential = new Credential(ViewModel.UserName, PasswordTextBox.Password, credentialTarget) { PersistanceType = PersistanceType.LocalComputer };
                            credential.Save();
                        }
                        else
                        {
                            var credential = new Credential() { Target = credentialTarget };
                            credential.Delete();
                        }

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

        public new bool? ShowDialog()
        {
            if (ViewModel == null)
            {
                return false;
            }

#if DEBUG
            ViewModel.UserName = "Administrator";
            ViewModel.SetPassword("administrator");
#endif

            ViewModel.Load();
            var storedCredentials = (new Credential() { Target = credentialTarget });
            if (storedCredentials.Load())
            {
                ViewModel.UserName = storedCredentials.Username;
                ViewModel.SetPassword(PasswordTextBox.Password = storedCredentials.Password);
                ViewModel.RememberMe = true;
            }
            else
            {
                ViewModel.RememberMe = false;
            }

            return base.ShowDialog();
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
                var content = new RegisterUserControl();

                if (content.DataContext is CreateUserViewModel createUserVM)
                {
                    createWindow.ModalContent = content;

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

        private void InputBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (sender is TextBox textBox)
                {
                    var request = new TraversalRequest(FocusNavigationDirection.Next);
                    textBox?.MoveFocus(request);
                    e.Handled = true;
                }
                else if (sender is PasswordBox pwBox)
                {
                    SubmitButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    e.Handled = true;
                }
            }
        }
    }
}
