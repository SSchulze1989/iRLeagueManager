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
    /// Interaktionslogik für ReviewResultControl.xaml
    /// </summary>
    public partial class ReviewResultControl : UserControl, IModalContent
    {
        public IncidentReviewViewModel ViewModel => DataContext as IncidentReviewViewModel;

        public ReviewResultControl()
        {
            InitializeComponent();
        }

        public string Header => "Edit Result";

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
            
        }

        public Task<bool> OnSubmitAsync()
        {
            return Task.FromResult(true);
        }
    }
}
