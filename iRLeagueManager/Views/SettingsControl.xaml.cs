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

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        private SettingsPageViewModel ViewModel => DataContext as SettingsPageViewModel;
        private SeasonViewModel Season => (DataContext as SettingsPageViewModel)?.Season;

        private ReadOnlyObservableCollection<ScoringViewModel> Scorings => (DataContext as SettingsPageViewModel)?.Scorings;

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
                var editWindow = EditPanel;
                var content = new PointsEditControl();

                editWindow.Title = "Edit Base points";

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
        }
        private void EditBonusPointsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag != null)
            {
                //var editWindow = new ModalOkCancelWindow();
                //editWindow.Width = 250;
                //editWindow.Height = 500;
                var editWindow = EditPanel;
                var content = new PointsEditControl();

                editWindow.Title = "Edit Bonus points";

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
        }

        private void DeleteScoringButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ScoringViewModel scoringViewModel && ViewModel != null)
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
            if (sender is Button button && button.Tag is ScoringTableViewModel scoringTableViewModel && ViewModel != null)
            {
                if (MessageBox.Show($"Would you really like to delete the Scoring Table \"{ scoringTableViewModel.Name}\"?\nThis action can not be undone!", "Delete ScoringTable", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ViewModel.DeleteScoringTable(scoringTableViewModel.Model);
                }
                e.Handled = true;
            }
        }
    }
}
