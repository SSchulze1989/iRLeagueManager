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
using iRLeagueManager.ViewModels;
using iRLeagueManager.Models.Members;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für EditTeamControl.xaml
    /// </summary>
    public partial class EditTeamControl : UserControl
    {
        public EditTeamControl()
        {
            InitializeComponent();
        }

        private void MoveLeftButton_Click(object sender, RoutedEventArgs e)
        {
            var team = DataContext as TeamViewModel;
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
            var team = DataContext as TeamViewModel;
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
