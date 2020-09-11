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
        public new WindowStartupLocation WindowStartupLocation = WindowStartupLocation.CenterOwner;

        public ModalOkCancelWindow()
        {
            InitializeComponent();
        }

        private async void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ModalContent.Content is IModalContent modalContent)
            {
                if (modalContent.CanSubmit())
                {
                    var result = await modalContent.OnSubmitAsync();

                    if (result == true)
                    {
                        DialogResult = result;
                        Close();
                    }
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
            if (ModalContent.Content is IModalContent modalContent)
            {
                modalContent.OnCancel();
            }
            DialogResult = false;
            Close();
        }
    }
}
