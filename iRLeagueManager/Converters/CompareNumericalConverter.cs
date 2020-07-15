using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using iRLeagueManager.Enums;

namespace iRLeagueManager.Converters
{
    public class CompareNumericalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string parameterString)
            {
                int.TryParse(parameterString, out int parameterValue);
                parameter = parameterValue;
            }

            if(value is IComparable comparable && parameter is IComparable compareTo)
            {
                return comparable.CompareTo(compareTo);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
