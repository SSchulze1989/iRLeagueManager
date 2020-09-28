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
