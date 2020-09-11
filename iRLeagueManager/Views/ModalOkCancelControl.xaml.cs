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
using System.Windows.Threading;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für ModalOkCancelControl.xaml
    /// </summary>
    public partial class ModalOkCancelControl : UserControl
    {
        //private Task blockTask;
        public static DependencyProperty IsDialogOpenProperty =
            DependencyProperty.Register(nameof(IsDialogOpen), typeof(bool), typeof(ModalOkCancelControl),
                new PropertyMetadata(false));

        public static DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(ModalOkCancelControl),
                new PropertyMetadata("Dialog Control"));
        
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        
        public bool IsDialogOpen 
        {
            get => (bool)GetValue(IsDialogOpenProperty);
            private set => SetValue(IsDialogOpenProperty, value); 
        }
        public bool? DialogResult { get; private set; }

        private event EventHandler DialogClosed;

        public ModalOkCancelControl()
        {
            IsDialogOpen = false;
            InitializeComponent();
        }

        public bool? ShowDialog()
        {
            IsDialogOpen = true;
            var frame = new DispatcherFrame();
            this.DialogClosed += (sender, args) =>
            {
                frame.Continue = false; // stops the frame
            };
            Dispatcher.PushFrame(frame);

            return DialogResult;
        }

        public void Close()
        {
            DialogClosed?.Invoke(this, new EventArgs());
            IsDialogOpen = false;
        }

        private async void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ModalContent.Content is IModalContent modalContent)
            {
                if (modalContent.CanSubmit())
                {
                    var result = await modalContent.SubmitAsync();

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
                modalContent.Cancel();
            }
            DialogResult = false;
            Close();
        }
    }
}
