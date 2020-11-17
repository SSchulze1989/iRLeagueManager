using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach(var item in enumerable)
            {
                action?.Invoke(item);
            }
        }

        public static double WeightedAverage<T>(this IEnumerable<T> enumerable, Func<T, double> value, Func<T, double> weight)
        {
            // Calculate weighted average by dividing the sum of products between weight and value by the total sum of all weights.
            var totalWeight = enumerable.Sum(x => weight(x));
            return (enumerable.Sum(x => value(x) * weight(x)) / totalWeight).GetZeroWhenInvalid();
        }
    }
}
