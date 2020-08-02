using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace iRLeagueManager.Controls
{
    public class IconButton : Button
    {
        public static DependencyProperty IconContentProperty =
            DependencyProperty.Register(nameof(IconContent), typeof(object), typeof(IconButton), 
                new PropertyMetadata(null));

        public static DependencyProperty StackOrientationProperty =
            DependencyProperty.Register(nameof(StackOrientation), typeof(Orientation), typeof(IconButton),
                new PropertyMetadata(Orientation.Horizontal));

        public static DependencyProperty StackFlowDirectionProperty =
            DependencyProperty.Register(nameof(StackFlowDirection), typeof(FlowDirection), typeof(IconButton),
                new PropertyMetadata(FlowDirection.LeftToRight));

        public object IconContent 
        { 
            get => (object)GetValue(IconContentProperty); 
            set => SetValue(IconContentProperty, value); 
        }

        public Orientation StackOrientation
        {
            get => (Orientation)GetValue(StackOrientationProperty);
            set => SetValue(StackOrientationProperty, value);
        }

        public FlowDirection StackFlowDirection
        {
            get => (FlowDirection)GetValue(StackFlowDirectionProperty);
            set => SetValue(StackFlowDirectionProperty, value);
        }
    }
}
