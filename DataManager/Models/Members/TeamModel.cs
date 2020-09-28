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

namespace iRLeagueManager.Models.Members
{
    public class TeamModel : MappableModel, ICacheableModel
    {
        public override long[] ModelId => new long[] { TeamId };

        public long TeamId { get; internal set; }

        private string name;
        public string Name { get => name; set => SetValue(ref name, value); }

        private string profile;
        public string Profile { get => profile; set => SetValue(ref profile, value); }

        private string teamColor;
        public string TeamColor { get => teamColor; set => SetValue(ref teamColor, value); }

        private string teamHomepage;
        public string TeamHomepage { get => teamColor; set => SetValue(ref teamHomepage, value); }
        //public long[] MemberIds { get; set; }

        private ObservableCollection<LeagueMember> members;
        public ObservableCollection<LeagueMember> Members { get => members; set => SetNotifyCollection(ref members, value); }

        public TeamModel()
        {
            Members = new ObservableCollection<LeagueMember>();
        }
    }
}
