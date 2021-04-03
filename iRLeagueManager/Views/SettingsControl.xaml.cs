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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

using iRLeagueManager.ViewModels;
using System.Windows.Controls.Primitives;
using System.Diagnostics.Eventing.Reader;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        private SettingsPageViewModel ViewModel => DataContext as SettingsPageViewModel;
        private SeasonViewModel Season => (DataContext as SettingsPageViewModel)?.Season;

        //private ReadOnlyObservableCollection<ScoringViewModel> Scorings => (DataContext as SettingsPageViewModel)?.Scorings;

        public SettingsControl()
        {
            InitializeComponent();
        }

        private void EditBasePointsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag != null)
            {
                //var editWindow = new ModalOkCancelWindow();
                //editWindow.Width = 250;
                //editWindow.Height = 500;
                var EditPanel = new ModalOkCancelControl();
                MainGrid.Children.Add(EditPanel);
                var editWindow = EditPanel;
                var content = new PointsEditControl();

                editWindow.Title = "Edit Base points";

                try
                {
                    if (button.Tag is ScoringViewModel scoring && scoring != null)
                    {
                        var editBasePoints = new ObservableCollection<iRLeagueManager.Models.Results.ScoringModel.BasePointsValue>(scoring.BasePoints.ToList());
                        content.DataContext = editBasePoints;
                        editWindow.ModalContent = content;

                        if (editWindow.ShowDialog() == true)
                        {
                            int i;
                            for (i = 0; i < editBasePoints.Count(); i++)
                            {
                                if (i >= scoring.BasePoints.Count())
                                    scoring.BasePoints.Add(editBasePoints[i]);
                                else
                                    scoring.BasePoints[i] = editBasePoints[i];
                            }
                            for (i = editBasePoints.Count(); i < scoring.BasePoints.Count(); i++)
                            {
                                scoring.BasePoints.RemoveAt(i);
                            }
                        }
                    }
                }
                finally
                {
                    MainGrid.Children.Remove(EditPanel);
                }
            }
        }
        private void EditBonusPointsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag != null)
            {
                //var editWindow = new ModalOkCancelWindow();
                //editWindow.Width = 250;
                //editWindow.Height = 500;
                var EditPanel = new ModalOkCancelControl();
                MainGrid.Children.Add(EditPanel);
                var editWindow = EditPanel;
                var content = new PointsEditControl();

                editWindow.Title = "Edit Bonus points";

                try
                {
                    if (button.Tag is ScoringViewModel scoring && scoring != null)
                    {
                        var editBonusPoints = new ObservableCollection<iRLeagueManager.Models.Results.ScoringModel.BonusPointsValue>(scoring.BonusPoints.ToList());
                        content.DataContext = editBonusPoints;
                        editWindow.ModalContent = content;

                        if (editWindow.ShowDialog() == true)
                        {
                            int i;
                            for (i = 0; i < editBonusPoints.Count(); i++)
                            {
                                if (i >= scoring.BonusPoints.Count())
                                    scoring.BonusPoints.Add(editBonusPoints[i]);
                                else
                                    scoring.BonusPoints[i] = editBonusPoints[i];
                            }
                            for (i = editBonusPoints.Count(); i < scoring.BonusPoints.Count(); i++)
                            {
                                scoring.BonusPoints.RemoveAt(i);
                            }
                        }
                    }
                }
                finally
                {
                    MainGrid.Children.Remove(EditPanel);
                }
            }
        }

        private void DeleteScoringButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Control control && control.Tag is ScoringViewModel scoringViewModel && ViewModel != null)
            {
                if (MessageBox.Show($"Would you really like to delete th Scoring \"{scoringViewModel.Name}\"?\nThis action can not be undone!", "Delete Scoring", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                    ViewModel.DeleteScoring(scoringViewModel.Model);
                }
                e.Handled = true;
            }
        }

        private void MoveLeftButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MoveRightButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ScoringList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveRightButton.Command.Execute(MoveRightButton.CommandParameter);
            e.Handled = true;
        }

        private void ScoringSelect_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveLeftButton.Command.Execute(MoveLeftButton.CommandParameter);
            e.Handled = true;
        }

        private void DeleteScoringTableButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Control control && control.Tag is ScoringTableViewModel scoringTableViewModel && ViewModel != null)
            {
                if (MessageBox.Show($"Would you really like to delete the Scoring Table \"{ scoringTableViewModel.Name}\"?\nThis action can not be undone!", "Delete ScoringTable", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ViewModel.DeleteScoringTable(scoringTableViewModel.Model);
                }
                e.Handled = true;
            }
        }

        private async void EditFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            var EditPanel = new ModalOkCancelControl();
            MainGrid.Children.Add(EditPanel);
            try
            {
                if (sender is Button button && button.Tag is ScoringViewModel scoringViewModel)
                {
                    var editWindow = EditPanel;
                    var filterEdit = new FiltersEditControl();
                    await filterEdit.ViewModel.Load(scoringViewModel.Model);
                    editWindow.ModalContent = filterEdit;

                    editWindow.ShowDialog();
                }
            }
            finally
            {
                MainGrid.Children.Remove(EditPanel);
            }
        }

        private void StatisticSetSelect_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is Button button)
            {
                button.Command?.Execute(button.CommandParameter);
            }
        }

        private void SessionSelect_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is Button button)
            {
                button.Command?.Execute(button.CommandParameter);
            }
        }
    }
}
