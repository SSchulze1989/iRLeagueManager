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
    /// Interaktionslogik für CommentEditControl.xaml
    /// </summary>
    public partial class CommentEditControl : UserControl, IModalContent
    {
        public CommentEditControl()
        {
            InitializeComponent();
        }

        public string Header { get; set; } = "Edit Comment";

        public string SubmitText { get; set; } = "Edit";

        public string CancelText { get; set; } = "Cancel";

        public bool IsLoading => false;

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

        public async Task<bool> OnSubmitAsync()
        {
            return true;
        }
    }
}
