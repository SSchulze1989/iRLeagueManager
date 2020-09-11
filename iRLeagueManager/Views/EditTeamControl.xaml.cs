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
    public partial class EditTeamControl : UserControl, IModalContent
    {
        public EditTeamControl()
        {
            InitializeComponent();
        }

        public string Header { get; set; } = "Edit Team Data";

        public string SubmitText { get; set; } = "Save";

        public string CancelText { get; set; } =  "Cancel";

        public void OnCancel()
        {
        }

        public bool CanSubmit()
        {
            return IsValid(this);
        }

        private bool IsValid(DependencyObject obj)
        {
            // The dependency object is valid if it has no errors and all
            // of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(obj) &&
            LogicalTreeHelper.GetChildren(obj)
            .OfType<DependencyObject>()
            .All(IsValid);
        }

        public async Task<bool> OnSubmitAsync()
        {
            if (DataContext is TeamViewModel teamViewModel)
            {
                if (teamViewModel.TeamColor == null || teamViewModel.TeamColor == "")
                    teamViewModel.TeamColor = "#666666";
            }
            return true;
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
