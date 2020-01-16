using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Attributes;
using iRLeagueManager.Enums;

namespace iRLeagueManager.Interfaces
{
    public interface ILeague
    {
        string Name { get; }
        IEnumerable<ILeagueMember> LeagueMembers { get; }
        IEnumerable<IAdmin> Admins { get; }
        IEnumerable<ISeasonInfo> Seasons { get; }
        IEnumerable<ITeam> Teams { get; }

        ILeagueMember AddNewMember(uint memberId, string firstname = "Firstname", string lastname = "Lastname");
        ILeagueMember AddNewMember(string firstname = "firstname", string lastname = "lastname");
        bool AddExistingMember(ILeagueMember member);
        bool RemoveMember(ILeagueMember member);

        IAdmin MakeAdmin(ILeagueMember member, AdminRights rights);
        bool GiveAdminRights(IAdmin admin, AdminRights rights);
        bool RevokeAdminRights(IAdmin admin, AdminRights rights);

        ISeason AddNewSeason();
        ISeason GetSeason(uint seasonId);
        ISeason GetSeason(ISeasonInfo seasonInfo);
        bool AddExistingSeason(ISeason season);
        bool RemoveSeason(ISeason season);

        ITeam AddNewTeam();
        bool AddExistingTeam(ITeam team);
        bool RemoveTeam(ITeam team);

        void SaveData();
        void LoadData();
    }
}
