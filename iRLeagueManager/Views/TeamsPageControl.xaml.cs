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
using iRLeagueManager.Models.Members;
using iRLeagueManager.ViewModels;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für TeamsPageControl.xaml
    /// </summary>
    public partial class TeamsPageControl : UserControl
    {
        public TeamsPageControl()
        {
            InitializeComponent();
        }

        private void MoveLeftButton_Click(object sender, RoutedEventArgs e)
        {
            var team = teamSelect.SelectedItem as TeamViewModel;
            if (sender is Button button && team != null)
            {
                var selectedMembers = MemberSelect.SelectedItems.Cast<LeagueMember>();

                if (selectedMembers != null && selectedMembers.Count() > 0)
                {
                    foreach (var selectedMember in selectedMembers.ToList())
                    {
                        team.AddMember(selectedMember);
                    }
                }
            }
        }

        private void MoveRightButton_Click(object sender, RoutedEventArgs e)
        {
            var team = teamSelect.SelectedItem as TeamViewModel;
            if (sender is Button button && team != null)
            {
                var selectedMembers = InvolvedMembers.SelectedItems.Cast<LeagueMember>();

                if (selectedMembers != null && selectedMembers.Count() > 0)
                {
                    foreach (var selectedMember in selectedMembers.ToList())
                    {
                        team.RemoveMember(selectedMember);
                    }
                }
            }
        }
    }
}
