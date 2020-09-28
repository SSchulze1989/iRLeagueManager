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
using System.Collections.ObjectModel;
using iRLeagueManager.Models.Members;
using System.Windows.Media;

namespace iRLeagueManager.ViewModels
{
    public class TeamViewModel : LeagueContainerModel<TeamModel>
    {
        protected override TeamModel Template => new TeamModel()
        {
            Name = "New Team",
            TeamColor = "#666666"
        };

        public long TeamId => Model.TeamId;
        public string Name
        {
            get => Model.Name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Teamname must not be empty");
                Model.Name = value;
            }
        }
        public string TeamColor
        {
            get => Model.TeamColor;
            set
            {
                if (ColorConverter.ConvertFromString(value) == null)
                    throw new ArgumentException("Please enter a valid color name or hex value (eg.\"#00ABFF\"");
                Model.TeamColor = value;
            }
        }

        public ObservableCollection<LeagueMember> Members => Model.Members;

        public MemberListViewModel MemberList { get; } = new MemberListViewModel();

        private string statusMessage;
        public string StatusMessage { get => statusMessage; set => SetValue(ref statusMessage, value); }

        public TeamViewModel()
        {
            MemberList.CustomFilters.Add(x => Members.Contains(x) == false);
            MemberList.CustomFilters.Add(x => x.Team == null);
        }

        public async void AddMember(LeagueMember member)
        {
            if (Members.Contains(member) == false)
            {
                Members.Add(member);
                member.Team = Model;
                await MemberList.Refresh();
            }
        }

        public async void RemoveMember(LeagueMember member)
        {
            if (Members.Contains(member))
            {
                Members.Remove(member);
                member.Team = null;
                await MemberList.Refresh();
            }
        }
    }
}
