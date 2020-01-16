using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Locations
{
    public class TrackCollection : IEnumerable<RaceTrack>
    {
        private LocationCollection Locations { get; }

        private IEnumerable<RaceTrack> Tracks => Locations.Select(x => x.GetTrackInfo()).Distinct();

        public IEnumerator<RaceTrack> GetEnumerator()
        {
            return Tracks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Tracks.GetEnumerator();
        }

        public TrackCollection()
        {
            Locations = new LocationCollection();
        }
    }
}
