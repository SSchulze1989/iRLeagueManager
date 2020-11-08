// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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

        public bool IsDialogOpen { get; set; }

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

            if (IsDialogOpen == false && ModalContent is IModalContent modalContent)
                modalContent.OnLoad();

            IsDialogOpen = true;

            if (isRendered)
                return;

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
