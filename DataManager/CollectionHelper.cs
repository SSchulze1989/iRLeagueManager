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
