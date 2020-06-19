using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using iRLeagueManager.Timing;

namespace iRLeagueManager.Converters
{
    public class LapIntervalStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LapInterval interval)
            {
                if (interval.Laps > 0)
                    return "+" + interval.Laps.ToString() + "L";
                else if (interval.Laps < 0)
                    return interval.Laps.ToString() + "L";
                else if (Math.Abs(interval.Time.Minutes) >= 1)
                    return String.Format(@"{0}{1:##}:{2:00},{3:000}", Math.Sign(interval.Time.TotalSeconds) > 0 ? "+" : "-", Math.Abs(interval.Time.Minutes), Math.Abs(interval.Time.Seconds), Math.Abs(interval.Time.Milliseconds));
                else
                    return String.Format(@"{0}{1:#0},{2:000}", Math.Sign(interval.Time.TotalSeconds) > 0 ? "+" : "-", Math.Abs(interval.Time.Seconds), Math.Abs(interval.Time.Milliseconds));
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
