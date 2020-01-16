using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace iRLeagueManager
{
    public static class CollectionHelper
    {
        public static ICollection<TDest> UpdateCollection<TSource, TDest>(IEnumerable<TSource> source, ICollection<TDest> dest, Func<TSource, TDest, bool> comparer, Action<TSource, TDest> converter, Func<TSource, TDest> constructor)
        {
            List<TDest> addList = new List<TDest>();
            List<TDest> updateList = new List<TDest>();
            List<TDest> deleteList = new List<TDest>();
            foreach (var srcItem in source)
            {
                var destItem = dest.SingleOrDefault(x => comparer.Invoke(srcItem, x));
                if (destItem != null)
                {
                    converter.Invoke(srcItem, destItem);
                    updateList.Add(destItem);
                }
                else
                {
                    destItem = constructor.Invoke(srcItem);
                    addList.Add(destItem);
                    dest.Add(destItem);
                }
            }
            deleteList = dest.Where(x => !(updateList.Contains(x) || addList.Contains(x))).ToList();
            deleteList.ForEach(x => dest.Remove(x));
            return dest;
        }
    }
}
