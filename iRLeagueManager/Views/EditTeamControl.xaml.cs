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
using iRLeagueManager.ViewModels;
using iRLeagueManager.Models.Members;
using System.Windows.Controls.Primitives;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für EditTeamControl.xaml
    /// </summary>
    public partial class EditTeamControl : UserControl, IModalContent
    {
        public static DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(SelectLeagueControl),
                new PropertyMetadata(false));

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

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

        public Task<bool> OnSubmitAsync()
        {
            if (DataContext is TeamViewModel teamViewModel)
            {
                if (teamViewModel.TeamColor == null || teamViewModel.TeamColor == "")
                    teamViewModel.TeamColor = "#666666";
            }
            return Task.FromResult(true);
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
                var selectedMembers = TeamMembers.SelectedItems.Cast<LeagueMember>();

                if (selectedMembers != null && selectedMembers.Count() > 0)
                {
                    foreach (var selectedMember in selectedMembers.ToList())
                    {
                        team.RemoveMember(selectedMember);
                    }
                }
            }
        }

        private void TeamMembers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
    }
}
