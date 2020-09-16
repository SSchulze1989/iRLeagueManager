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
