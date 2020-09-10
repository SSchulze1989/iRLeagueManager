using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using iRLeagueManager.Models.Members;

namespace iRLeagueManager.ViewModels
{
    public class TeamViewModel : LeagueContainerModel<TeamModel>
    {
        protected override TeamModel Template => new TeamModel()
        {
            Name = "Template Team"
        };

        public long TeamId => Model.TeamId;
        public string Name { get => Model.Name; set => Model.Name = value; }
        public string TeamColor { get => Model.TeamColor; set => Model.TeamColor = value; }

        public ObservableCollection<LeagueMember> Members => Model.Members;

        public MemberListViewModel MemberList { get; } = new MemberListViewModel();

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
