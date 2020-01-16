using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using iRLeagueManager.Models;
using iRLeagueManager.Locations;

namespace iRLeagueManager
{
    public class LocationMapperProfile : Profile
    {
        private IEnumerable<Location> LocationCollection { get; }

        public LocationMapperProfile(IEnumerable<Location> locationCollection)
        {
            LocationCollection = locationCollection;
            CreateMap<string, Location>()
                .ConvertUsing(src => GetLocationFromString(src));

            CreateMap<Location, string>()
                .ConvertUsing(src => GetLocationId(src));
        }

        private Location GetLocationFromString(string str)
        {
            var strArray = str.Split('-');

            if (strArray.Count() != 2)
                return null;

            if (!int.TryParse(strArray[0], out var trackId) || !int.TryParse(strArray[1], out var configId))
                return null;

            return LocationCollection.SingleOrDefault(x => x.GetConfigInfo().ConfigId == configId && x.GetTrackInfo().TrackId == trackId);
        }

        private string GetLocationId(Location location)
        {
            if (location == null)
                return "";

            return location.LocationId;
        }
    }
}
