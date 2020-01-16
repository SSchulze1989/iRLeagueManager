using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Converters;

namespace iRLeagueManager.ViewModels
{
    public class HoursCollection : IList<string>
    {
        private List<string> hours = new List<string>();

        public HoursCollection()
        {
            TimeComponentConverter converter = new TimeComponentConverter();
            for (int i = 0; i < 25; i++)
            {
                hours.Add((string)converter.Convert(i, typeof(string), null, System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        public string this[int index] { get => ((IList<string>)hours)[index]; set => ((IList<string>)hours)[index] = value; }

        public int Count => ((IList<string>)hours).Count;

        public bool IsReadOnly => ((IList<string>)hours).IsReadOnly;

        public void Add(string item)
        {
            ((IList<string>)hours).Add(item);
        }

        public void Clear()
        {
            ((IList<string>)hours).Clear();
        }

        public bool Contains(string item)
        {
            return ((IList<string>)hours).Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            ((IList<string>)hours).CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IList<string>)hours).GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return ((IList<string>)hours).IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            ((IList<string>)hours).Insert(index, item);
        }

        public bool Remove(string item)
        {
            return ((IList<string>)hours).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<string>)hours).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<string>)hours).GetEnumerator();
        }
    }
}
