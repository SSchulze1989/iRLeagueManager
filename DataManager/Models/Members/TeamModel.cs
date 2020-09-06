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
