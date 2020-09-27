using iRLeagueManager.Models.Members;
using iRLeagueManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaktionslogik für ReviewEditControl.xaml
    /// </summary>
    public partial class ReviewEditControl : UserControl, IModalContent
    {
        public IncidentReviewViewModel IncidentReview => DataContext as IncidentReviewViewModel;

        public string Header { get; set; } = "Edit Review";

        public string SubmitText { get; set; } = "Save";

        public string CancelText { get; set; } = "Cancel";

        public bool IsLoading { get; set; }

        public ReviewEditControl()
        {
            InitializeComponent();
        }

        private void MoveLeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && IncidentReview != null)
            {
                var selectedMembers = MemberSelect.SelectedItems.Cast<LeagueMember>();

                if (selectedMembers != null && selectedMembers.Count() > 0)
                {
                    foreach (var selectedMember in selectedMembers.ToList())
                    {
                        IncidentReview.AddMember(selectedMember);
                    }
                }
            }
        }

        private void MoveRightButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && IncidentReview != null)
            {
                var selectedMembers = InvolvedMembers.SelectedItems.Cast<LeagueMember>();

                if (selectedMembers != null && selectedMembers.Count() > 0)
                {
                    foreach (var selectedMember in selectedMembers.ToList())
                    {
                        IncidentReview.RemoveMember(selectedMember);
                    }
                }
            }
        }

        private void InvolvedMembers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveRightButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            e.Handled = true;
        }

        private void MemberSelect_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveLeftButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            e.Handled = true;
        }

        public void OnLoad()
        {
        }

        public bool CanSubmit()
        {
            return true;
        }

        public async Task<bool> OnSubmitAsync()
        {
            return true;
        }

        public void OnCancel()
        {
        }
    }
}
