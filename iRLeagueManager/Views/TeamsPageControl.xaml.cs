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
        private TeamsPageViewModel TeamsPageViewModel => DataContext as TeamsPageViewModel;

        public TeamsPageControl()
        {
            InitializeComponent();
        }

        //private void MoveLeftButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var team = teamSelect.SelectedItem as TeamViewModel;
        //    if (sender is Button button && team != null)
        //    {
        //        var selectedMembers = MemberSelect.SelectedItems.Cast<LeagueMember>();

        //        if (selectedMembers != null && selectedMembers.Count() > 0)
        //        {
        //            foreach (var selectedMember in selectedMembers.ToList())
        //            {
        //                team.AddMember(selectedMember);
        //            }
        //        }
        //    }
        //}

        //private void MoveRightButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var team = teamSelect.SelectedItem as TeamViewModel;
        //    if (sender is Button button && team != null)
        //    {
        //        var selectedMembers = InvolvedMembers.SelectedItems.Cast<LeagueMember>();

        //        if (selectedMembers != null && selectedMembers.Count() > 0)
        //        {
        //            foreach (var selectedMember in selectedMembers.ToList())
        //            {
        //                team.RemoveMember(selectedMember);
        //            }
        //        }
        //    }
        //}

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var editWindow = EditPanel;
                var content = new EditTeamControl()
                {
                    Header = "Add new Team",
                    SubmitText = "Add"
                };
                
                if (content.DataContext is TeamViewModel editVM)
                {
                    editVM.Model = new TeamModel()
                    {
                        Name = "New Team",
                        TeamColor = "#666666"
                    };

                    editWindow.ModalContent.Content = content;
                    if (editWindow.ShowDialog() == true)
                    {
                        TeamsPageViewModel?.AddTeam(editVM.Model);
                    }
                }
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TeamViewModel teamVM)
            {
                //var editWindow = new ModalOkCancelWindow();
                var editWindow = EditPanel;
                //editWindow.Width = 700;
                //editWindow.Height = 700;
                var content = new EditTeamControl();

                editWindow.Title = "Edit Team Data";

                if (content.DataContext is TeamViewModel editVM)
                {
                    editVM.Model.CopyFrom(teamVM.Model);
                    //await editVM.LoadMemberListAsync();

                    editWindow.ModalContent.Content = content;
                    if (editWindow.ShowDialog() == true)
                    {
                        teamVM.Model.CopyFrom(editVM.Model);
                        teamVM.SaveChanges();
                    }
                }
            }
        }
    }
}
