using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.ObjectModel;

namespace iRLeagueManager.Converters
{
    public class BonusPointsConverter : IValueConverter<string, ObservableCollection<KeyValuePair<string, int>>>, IValueConverter<IEnumerable<KeyValuePair<string, int>>, string>
    {
        ObservableCollection<KeyValuePair<string, int>> IValueConverter<string, ObservableCollection<KeyValuePair<string, int>>>.Convert(string sourceMember, ResolutionContext context)
        {
            ObservableCollection<KeyValuePair<string, int>> target = new ObservableCollection<KeyValuePair<string, int>>();
            return target;
        }

        string IValueConverter<IEnumerable<KeyValuePair<string, int>>, string>.Convert(IEnumerable<KeyValuePair<string, int>> sourceMember, ResolutionContext context)
        {
            return "";
        }
    }
}
