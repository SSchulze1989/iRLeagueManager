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
    /// Interaktionslogik für CreateSeasonControl.xaml
    /// </summary>
    public partial class CreateSeasonControl : UserControl, IModalContent
    {
        private MainWindowViewModel MainWindowVM { get; }

        public string Header => "Create new Season";

        public string SubmitText => "Create";

        public string CancelText => "Cancel";

        public bool IsLoading { get; private set; }

        public CreateSeasonControl(MainWindowViewModel mainWindowVM)
        {
            MainWindowVM = mainWindowVM;
            InitializeComponent();
        }

        public bool CanSubmit()
        {
            return string.IsNullOrEmpty(SeasonNameTextBox.Text) == false;
        }

        public void OnCancel()
        {
        }

        public void OnLoad()
        {
        }

        public async Task<bool> OnSubmitAsync()
        {
            return await MainWindowVM.AddSeason(new Models.SeasonModel() { SeasonName = SeasonNameTextBox.Text }) != null;
        }
    }
}
