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
    /// Interaktionslogik für SessionEditControl.xaml
    /// </summary>
    public partial class SessionEditControl : UserControl
    {
        private SessionViewModel ViewModel => this.DataContext as SessionViewModel;

        public SessionEditControl()
        {
            InitializeComponent();
        }
    }
}
