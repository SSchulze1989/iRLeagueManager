using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using iRLeagueManager.Data;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.LeagueDBServiceRef;
using AutoMapper;

namespace iRLeagueManager
{
    public class SeasonContext
    {
        private ModelMapperProfile MapperProfile { get; }

        private MapperConfiguration MapperConfiguration { get; }

        private readonly ILeagueDBService leagueDBService;

        private readonly SeasonModel season;

        //private ObservableCollection<LeagueMember> memberList;
        //public ObservableCollection<LeagueMember> MemberList => memberList;

        internal SeasonContext(ILeagueDBService dBService, SeasonModel seasonModel)
        {
            leagueDBService = dBService;
            MapperProfile = new ModelMapperProfile();
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(MapperProfile);
            });
            season = seasonModel;
        }
    }
}
