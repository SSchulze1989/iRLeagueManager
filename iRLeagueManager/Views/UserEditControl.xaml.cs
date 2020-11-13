using iRLeagueManager.Extensions;
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
    /// Interaktionslogik für UserEditControl.xaml
    /// </summary>
    public partial class UserEditControl : UserControl, IModalContent
    {
        public UserViewModel ViewModel => DataContext as UserViewModel; 

        public UserEditControl()
        {
            InitializeComponent();
        }

        public string Header => "Edit User Profile";

        public string SubmitText => "Save";

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
            DataContext = new UserViewModel();
            ViewModel.UpdateSource(GlobalSettings.LeagueContext.UserManager.CurrentUser);
        }

        public async Task<bool> OnSubmitAsync()
        {
            if (ViewModel == null || this.IsValid() == false)
            {
                return false;
            }

            if (this.IsValid())
            {
                return await ViewModel.SaveChanges();
            }
            return false;
        }
    }
}
