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
using System.ComponentModel;
using iRLeagueManager.ViewModels;
using System.Runtime.CompilerServices;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaction logic for RegisterUserControl.xaml
    /// </summary>
    public partial class RegisterUserControl : UserControl, IModalContent, INotifyPropertyChanged
    {
        public CreateUserViewModel ViewModel => DataContext as CreateUserViewModel;

        public bool IsLoading => (ViewModel != null) ? ViewModel.IsLoading : false;

        public string Header => "Register new User";

        public string SubmitText => "Register";

        public string CancelText => "Cancel";

        public event PropertyChangedEventHandler PropertyChanged;

        public RegisterUserControl()
        {
            DataContextChanged += OnDataContextChanged;
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ViewModel != null)
                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            if (ViewModel == null)
                return false;

            return ViewModel.CanSubmit();
        }

        public async Task<bool> OnSubmitAsync()
        {
            return await ViewModel.SubmitAsync();
        }

        public void OnCancel()
        {
            ViewModel.Dispose();
        }

        public void OnLoad()
        {
        }
    }
}
