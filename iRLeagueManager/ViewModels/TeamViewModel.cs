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

        public void AddMember(LeagueMember member)
        {
            if (Members.Contains(member) == false)
            {
                Members.Add(member);
                member.Team = Model;
                MemberList.Refresh();
            }
        }

        public void RemoveMember(LeagueMember member)
        {
            if (Members.Contains(member))
            {
                Members.Remove(member);
                member.Team = null;
                MemberList.Refresh();
            }
        }
    }
}
