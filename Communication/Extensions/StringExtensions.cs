using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Parse int or give default value when failed
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ParseIntOrDefault(this string str)
        {
            return int.TryParse(str, out int res) ? res : default;
        }

        /// <summary>
        /// Parse double or give default value when failed
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ParseDoubleOrDefault(this string str)
        {
            return double.TryParse(str, out double res) ? res : default;
        }
    }
}
