using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace iRLeagueManager.Converters
{
    public class ColorComplementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double threshold = 0.5;
            if (parameter != null)
            {
                double.TryParse(parameter.ToString(), NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out threshold);
            }

            if (value is SolidColorBrush brush)
            {
                var mediaColor = brush.Color;
                System.Drawing.Color drawingColor = System.Drawing.Color.FromArgb(mediaColor.R, mediaColor.G, mediaColor.B);
                var brightness = drawingColor.GetBrightness();
                if (brightness > threshold)
                    return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                else
                    return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
            else if (value is string valueString)
            {
                var mediaColor = (Color)ColorConverter.ConvertFromString(valueString);
                System.Drawing.Color drawingColor = System.Drawing.Color.FromArgb(mediaColor.R, mediaColor.G, mediaColor.B);
                var brightness = drawingColor.GetBrightness();
                if (brightness > threshold)
                    return "Black";
                else
                    return "White";
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
