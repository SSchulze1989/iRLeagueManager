using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Converters;

namespace iRLeagueManager.ViewModels
{
    public class MinutesCollection : IList<string>
    {
        private List<string> minutes = new List<string>();

        public MinutesCollection()
        {
            TimeComponentConverter converter = new TimeComponentConverter();
            for (int i = 0; i < 60; i+=5)
            {
                minutes.Add((string)converter.Convert(i, typeof(string), null, System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        public string this[int index] { get => ((IList<string>)minutes)[index]; set => ((IList<string>)minutes)[index] = value; }

        public int Count => ((IList<string>)minutes).Count;

        public bool IsReadOnly => ((IList<string>)minutes).IsReadOnly;

        public void Add(string item)
        {
            ((IList<string>)minutes).Add(item);
        }

        public void Clear()
        {
            ((IList<string>)minutes).Clear();
        }

        public bool Contains(string item)
        {
            return ((IList<string>)minutes).Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            ((IList<string>)minutes).CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IList<string>)minutes).GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return ((IList<string>)minutes).IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            ((IList<string>)minutes).Insert(index, item);
        }

        public bool Remove(string item)
        {
            return ((IList<string>)minutes).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<string>)minutes).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<string>)minutes).GetEnumerator();
        }
    }
}
