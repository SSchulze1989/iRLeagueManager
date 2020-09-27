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
        public static readonly DependencyProperty ShowHeaderProperty =
            DependencyProperty.Register(nameof(ShowHeader), typeof(bool), typeof(ModalOkCancelWindow),
                new PropertyMetadata(true));

        public static DependencyProperty ModalContentProperty =
            DependencyProperty.Register(nameof(ModalContent), typeof(UserControl), typeof(ModalOkCancelWindow),
                new PropertyMetadata(null));

        public UserControl ModalContent
        {
            get => (UserControl)GetValue(ModalContentProperty);
            set => SetValue(ModalContentProperty, value);
        }

        public bool CanReattach => windowReattaching != null;

        public bool ShowHeader
        {
            get => (bool)GetValue(ShowHeaderProperty);
            set => SetValue(ShowHeaderProperty, value);
        }

        private EventHandler windowReattaching;
        public event EventHandler WindowReattaching
        {
            add { windowReattaching += value; }
            remove { windowReattaching -= value; }
        }

        private bool isRendered;

        public ModalOkCancelWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (isRendered)
                return;

            if (ModalContent is IModalContent modalContent)
                modalContent.OnLoad();

            isRendered = true;
        }

        private async void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ModalContent is IModalContent modalContent)
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
            if (ModalContent is IModalContent modalContent)
            {
                modalContent.OnCancel();
            }
            DialogResult = false;
            Close();
        }

        private void ReAttachButton_Click(object sender, RoutedEventArgs e)
        {
            windowReattaching?.Invoke(sender, e);
        }
    }
}
