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

        public Task<bool> OnSubmitAsync()
        {
            return Task.FromResult(true);
        }

        public void OnCancel()
        {
        }
    }
}
