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
