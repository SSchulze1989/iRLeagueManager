using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Iterates over every item in the enumerable and executes the associated delegate action.
        /// </summary>
        /// <param name="action"><see langword="delegate"/> action to perform on each item.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action?.Invoke(item);
            }
        }

        /// <summary>
        /// Calculate weighted average over all selected values. 
        /// Values and weights are sleected from <typeparamref name="T"/> through delegate - both delegates must produce a valid result for all <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"><see langword="delegate"/> to select values for average</param>
        /// <param name="weight"><see langword="delegate"/> to select weights for average</param>
        /// <returns><see langword="value"/>; zero when not valid value could be calculated.</returns>
        public static double WeightedAverage<T>(this IEnumerable<T> enumerable, Func<T, double> value, Func<T, double> weight)
        {
            // Calculate weighted average by dividing the sum of products between weight and value by the total sum of all weights.
            var totalWeight = enumerable.Sum(x => weight(x));
            return (enumerable.Sum(x => value(x) * weight(x)) / totalWeight).GetZeroWhenInvalid();
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, params IEnumerable<T>[] others)
        {
            if (others == null || others.Count() == 0)
            {
                return first;
            }
            if (others.Count() == 1)
            {
                return Enumerable.Concat(first, others[0]);
            }
            return first.Concat(others[0].Concat(others.Skip(1).ToArray()));
        }

        public static bool Remove<T>(this ICollection<T> enumerable, IEnumerable<T> items)
        {
            bool result = true;
            foreach (var item in items)
            {
                result &= enumerable.Remove(item);
            }
            return result;
        }
    }
}
