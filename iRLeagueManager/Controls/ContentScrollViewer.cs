using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace iRLeagueManager.Controls
{
    public class ContentScrollViewer : ScrollViewer
    {
        public static readonly DependencyProperty ScrollableHeightProperty =
            DependencyProperty.Register(nameof(ScrollableHeight), typeof(double), typeof(ScrollViewer),
            new PropertyMetadata(0));

        public new double ScrollableHeight
        {
            get => (double)GetValue(ScrollableHeightProperty);
            set => SetValue(ScrollableHeightProperty, value);
        }

        protected override void OnScrollChanged(ScrollChangedEventArgs e)
        {
            ScrollableHeight = base.ScrollableHeight;
        }
    }
}
