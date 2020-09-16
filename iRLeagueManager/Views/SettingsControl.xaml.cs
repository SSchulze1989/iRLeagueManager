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

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
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
                    editWindow.ModalContent.Content = content;

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
                    editWindow.ModalContent.Content = content;

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

        private void MoveLeftButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MoveRightButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
