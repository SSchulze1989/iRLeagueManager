using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Extensions
{
    public static class SystemExtensions
    {
        /// <summary>
        /// Check if value is either Infinity or NaN and returns null in that case.
        /// </summary>
        /// <returns>0 or <paramref name="number"/></returns>
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
