using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Statistics
{
    public class ChampTitleModel : ModelBase
    {
        /// <summary>
        /// Name of the championship
        /// </summary>
        private string name;
        public string Name { get => name; set => SetValue(ref name, value); }
        /// <summary>
        /// Number of times this championship was won
        /// </summary>
        private int count;
        public int Count { get => count; set => SetValue(ref count, value); }
    }
}
