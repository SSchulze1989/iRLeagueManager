using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Database;

namespace iRLeagueManager.ViewModels
{
    public class LeagueDatabase : ContainerModelBase<ILeague>, ILeague
    {
        public string Name { get => Source.Name; }
        public IEnumerable<ILeagueMember> LeagueMembers { get => Source.LeagueMembers; }
        public IEnumerable<IAdmin> Admins { get => Source.Admins; }
        public IEnumerable<ISeasonInfo> Seasons { get => Source.Seasons; }
        public IEnumerable<ITeam> Teams { get => Source.Teams; }

        public LeagueDatabase(ILeague source) : base(source) { }

        public void SaveData()
        {
            Source.SaveData();
        }

        public void LoadData()
        {
            Source.LoadData();
        }

        public ILeagueMember AddNewMember(uint memberId, string firstname, string lastname)
        {
            return Source.AddNewMember(memberId, firstname, lastname);
        }

        public bool AddExistingMember(ILeagueMember member)
        {
            return Source.AddExistingMember(member);
        }

        public bool RemoveMember(ILeagueMember member)
        {
            return Source.RemoveMember(member);
        }

        public IAdmin MakeAdmin(ILeagueMember member, AdminRights rights)
        {
            return Source.MakeAdmin(member, rights);
        }

        public bool GiveAdminRights(IAdmin admin, AdminRights rights)
        {
            return Source.GiveAdminRights(admin, rights);
        }

        public bool RevokeAdminRights(IAdmin admin, AdminRights rights)
        {
            return Source.RevokeAdminRights(admin, rights);
        }

        public ISeason AddNewSeason()
        {
            return Source.AddNewSeason();
        }

        public bool AddExistingSeason(ISeason season)
        {
            return Source.AddExistingSeason(season);
        }

        public bool RemoveSeason(ISeason season)
        {
            return Source.RemoveSeason(season);
        }

        public ITeam AddNewTeam()
        {
            return Source.AddNewTeam();
        }

        public bool AddExistingTeam(ITeam team)
        {
            return Source.AddExistingTeam(team);
        }

        public bool RemoveTeam(ITeam team)
        {
            return Source.RemoveTeam(team);
        }

        public ISeason GetSeason(uint seasonId)
        {
            return Source.GetSeason(seasonId);
        }

        public ISeason GetSeason(ISeasonInfo seasonInfo)
        {
            return Source.GetSeason(seasonInfo);
        }

        public static LeagueDatabase LoadFromDb(string dbFolder, string leagueName)
        {
            IRDatabaseClient databaseClient = IRDatabaseClient.Load(dbFolder, new DatabaseUser(0, 0));
            //databaseClient.Save();
            return new LeagueDatabase(databaseClient.GetLeagueClient(leagueName));
        }

        public ILeagueMember AddNewMember(string firstname = "firstname", string lastname = "lastname")
        {
            return Source.AddNewMember(firstname, lastname);
        }
    }
}
