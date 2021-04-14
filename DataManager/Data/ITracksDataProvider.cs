using iRLeagueManager.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Data
{
    public interface ITracksDataProvider
    {
        Task<RaceTrack[]> GetRaceTracks();
    }
}
