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
    /// Interaktionslogik für ModalOkCancelWindow.xaml
    /// </summary>
    public partial class ModalOkCancelWindow : Window
    {
        public ModalOkCancelWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Content is IModalContent modalContent)
            {
                if (modalContent.CanSubmit())
                {
                    DialogResult = modalContent.Submit();
                    Close();
                }
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Content is IModalContent modalContent)
            {
                modalContent.Cancel();
            }
            DialogResult = false;
            Close();
        }
    }
}
