using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models
{
    public class LeagueModel : ModelBase
    {
        private string name;
        public string Name { get => name; set => SetValue(ref name, value); }

        public string longName;
        public string LongName { get => longName; set => SetValue(ref longName, value); }

    }
}
