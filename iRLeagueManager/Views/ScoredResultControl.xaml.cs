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

using iRLeagueManager.Models.Results;
using iRLeagueManager.Controls;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für ScoredResultControl.xaml
    /// </summary>
    public partial class ScoredResultControl : UserControl
    {
        public ResultsPageViewModel ResultsPage => DataContext as ResultsPageViewModel;

        public ScoredResultControl()
        {
            InitializeComponent();
        }

        private async void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ScoredResultRowViewModel resultRow) 
            {
                if (MessageBox.Show("Do you really want to remove this Row from the result Data set?\nThis action can not be undone!", "Delete Row", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await GlobalSettings.LeagueContext.DeleteModelsAsync<ResultRowModel>(new long[] { resultRow.Model.ResultRowId.GetValueOrDefault() });

                    if (ResultsPage != null)
                        await ResultsPage.LoadResults();
                }
            }
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = e.Source;


            var current = (DependencyObject)sender;
            var parent = VisualTreeHelper.GetParent(current);
            ScrollViewer scrollViewer = null;
            while(parent != null)
            {
                if (parent is ScrollViewer)
                    scrollViewer = (ScrollViewer)parent;
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (scrollViewer != null)
            {
                scrollViewer.RaiseEvent(eventArg);
                e.Handled = true;
            }
        }

        private void ShowDetailToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is  IconToggleButton button)
            {
                //Find parent DataGridRow
                DependencyObject findRow = button;
                while (findRow != null && findRow.GetType().Equals(typeof(DataGridRow)) == false)
                {
                    findRow = VisualTreeHelper.GetParent(findRow);
                }

                if (findRow is DataGridRow dataGridRow)
                {
                    switch (button.IsChecked)
                    {
                        case true:
                            dataGridRow.DetailsVisibility = Visibility.Visible;
                            break;
                        case false:
                            dataGridRow.DetailsVisibility = Visibility.Collapsed;
                            break;
                    }
                }
            }
        }
    }
}
