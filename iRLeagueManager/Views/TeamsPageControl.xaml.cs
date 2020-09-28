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
using iRLeagueManager.Models.Members;
using iRLeagueManager.ViewModels;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für TeamsPageControl.xaml
    /// </summary>
    public partial class TeamsPageControl : UserControl
    {
        private TeamsPageViewModel ViewModel => DataContext as TeamsPageViewModel;

        public TeamsPageControl()
        {
            InitializeComponent();
        }

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

                    editWindow.ModalContent = content;
                    if (editWindow.ShowDialog() == true)
                    {
                        await ViewModel?.AddTeam(editVM.Model);
                    }
                }
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TeamViewModel teamVM)
            {
                var editWindow = EditPanel;
                var content = new EditTeamControl();

                editWindow.Title = "Edit Team Data";

                if (content.DataContext is TeamViewModel editVM)
                {
                    editVM.Model.CopyFrom(teamVM.Model);

                    editWindow.ModalContent = content;
                    if (editWindow.ShowDialog() == true)
                    {
                        teamVM.Model.CopyFrom(editVM.Model);
                        await teamVM.SaveChanges();
                    }
                }
            }
        }

        private async void TeamDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TeamViewModel teamViewModel)
            {
                if (MessageBox.Show("Would you really like to delete this Team?\nThis action can not be undone!", "Delete Incident Review", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await ViewModel.RemoveTeam(teamViewModel.Model);
                }
                e.Handled = true;
            }
        }
    }
}
