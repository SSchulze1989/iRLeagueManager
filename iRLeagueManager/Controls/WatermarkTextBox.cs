using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace iRLeagueManager.Controls
{
    public static class WatermarkTextBox
    {
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(WatermarkTextBox),
                new PropertyMetadata(""));

        //public string Watermark
        //{
        //    get => (string)GetValue(WatermarkProperty);
        //    set => SetValue(WatermarkProperty, value);
        //}
    }
}
