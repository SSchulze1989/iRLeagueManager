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

using iRLeagueManager.Controls;
using iRLeagueManager.Extensions;
using iRLeagueManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaktionslogik für StandingsFiltersEditControl.xaml
    /// </summary>
    public partial class StandingsFiltersEditControl : UserControl, IModalContent
    {
        public StandingsFilterEditViewModel ViewModel => DataContext as StandingsFilterEditViewModel;

        public string Header { get; set; } = "Edit results Filters";

        public string SubmitText { get; set; } = "Save";

        public string CancelText { get; set; } = "Cancel";

        public bool IsLoading { get; private set; } = false;

        public StandingsFiltersEditControl()
        {
            DataContextChanged += StandingsFiltersEditControl_DataContextChanged;
            InitializeComponent();
        }

        private void StandingsFiltersEditControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.ViewOpenActionDialog += ViewModel_ViewOpenActionDialog;
            }
        }

        private void ViewModel_ViewOpenActionDialog(StandingsFilterEditViewModel sender, string title, string message, Action<StandingsFilterEditViewModel> okAction)
        {
            if (MessageBox.Show(message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                okAction?.Invoke(sender);
            }
        }

        public bool CanSubmit()
        {
            return true;
        }

        public void OnCancel()
        {
            return;
        }

        public async void OnLoad()
        {
            await ViewModel?.Refresh();
            return;
        }

        public async Task<bool> OnSubmitAsync()
        {
            if (ViewModel == null || this.IsValid() == false)
            {
                return false;
            }
            return await ViewModel.SaveChanges();
        }

        private void Comparator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.Tag is ContentControl contentControl)
            {
                contentControl.InvalidateProperty(ContentTemplateSelectorProperty);
            }
        }

        private void expandButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is IconToggleButton button && button.Tag is Popup popup)
            {
                popup.IsOpen = button.IsChecked;
            }
        }

        private void ListPopup_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void ListPopup_Closed(object sender, EventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is ResultsFilterOptionViewModel optionViewModel)
            {
                optionViewModel.RefreshFilterValueString();
            }

            if (sender is Popup popup && popup.Tag is IconToggleButton button)
            {
                button.IsChecked = false;
            }
        }
    }
}
