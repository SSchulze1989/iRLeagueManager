using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.Extensions
{
    public static class SystemExtensions
    {
        public static double GetZeroWhenInvalid(this double number)
        {
            if (double.IsInfinity(number) || double.IsNaN(number))
            {
                return 0;
            }
            return number;
        }
    }
}
