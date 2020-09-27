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

        public static DependencyProperty IsDetachedProperty =
            DependencyProperty.Register(nameof(IsDetached), typeof(bool), typeof(ModalOkCancelControl),
                new PropertyMetadata(false));

        public static DependencyProperty ModalContentProperty =
            DependencyProperty.Register(nameof(ModalContent), typeof(UserControl), typeof(ModalOkCancelControl),
                new PropertyMetadata(null));

        public UserControl ModalContent
        {
            get => (UserControl)GetValue(ModalContentProperty);
            set => SetValue(ModalContentProperty, value);
        }
        
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

        public bool IsDetached
        {
            get => (bool)GetValue(IsDetachedProperty);
            set => SetValue(IsDetachedProperty, value);
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
            InvalidateVisual();
            UpdateLayout();
            if (IsDetached)
            {
                var window = new ModalOkCancelWindow()
                {
                    Title = Title,
                    WindowStartupLocation = WindowStartupLocation.Manual
                };
                window.Height = popupGrid.ActualHeight + 40;
                window.Width = popupGrid.ActualWidth;

                PresentationSource source = PresentationSource.FromVisual(this);

                double dpiX = 96, dpiY = 96;
                if (source != null)
                {
                    dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                    dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
                }

                window.Top = popupGrid.PointToScreen(new Point(+20, +20)).Y / dpiY * 96;
                window.Left = popupGrid.PointToScreen(new Point(+20, +20)).X / dpiX * 96;

                var temp1 = ModalContent;
                ModalContent = null;
                window.ModalContent = temp1;
                window.WindowReattaching += (sender, args) =>
                {
                    IsDetached = false;
                    var temp2 = window.ModalContent;
                    window.ModalContent = null;
                    ModalContent = temp2;
                    window.Close();
                    popupGrid.Visibility = Visibility.Visible;
                };
                popupGrid.Visibility = Visibility.Collapsed;
                DialogResult = window.ShowDialog();
                if (IsDetached)
                {
                    popupGrid.Visibility = Visibility.Visible;
                    Close();
                    return DialogResult;
                }
                else
                {
                    return ShowDialog();
                }
            }
            else
            {
                var frame = new DispatcherFrame();
                this.DialogClosed += (sender, args) =>
                {
                    frame.Continue = false; // stops the frame
                };
                Dispatcher.PushFrame(frame);

                if (IsDetached == false)
                {
                    return DialogResult;
                }
                else
                {
                    return ShowDialog();
                }
            }
        }

        public void Close()
        {
            DialogClosed?.Invoke(this, new EventArgs());
            IsDialogOpen = false;
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

        private void DetachButton_Click(object sender, RoutedEventArgs e)
        {
            IsDetached = true;
            Close();
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var fullWidth = FullGrid.ActualWidth;
            var fullHeight = FullGrid.ActualHeight;

            var gridWidth = popupGrid.ActualWidth;
            var gridHeight = popupGrid.ActualHeight;

            var maxOffsetX = (fullWidth - gridWidth) / 2;
            var maxOffsetY = (fullHeight - gridHeight) / 2;

            PopupTranslate.X += e.HorizontalChange;
            if (PopupTranslate.X > maxOffsetX)
            {
                PopupTranslate.X = maxOffsetX;
            }
            else if (PopupTranslate.X < -maxOffsetX)
            {
                PopupTranslate.X = -maxOffsetX;
            }

            PopupTranslate.Y += e.VerticalChange;
            if (PopupTranslate.Y > maxOffsetY)
            {
                PopupTranslate.Y = maxOffsetY;
            }
            else if (PopupTranslate.Y < -maxOffsetY)
            {
                PopupTranslate.Y = -maxOffsetY;
            }

        }
    }
}
