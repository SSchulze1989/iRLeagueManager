using System;
using MS.Internal.PresentationFramework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;

namespace iRLeagueManager.Controls
{
    public class IconButton : Button
    {
        public static DependencyProperty IconContentProperty =
            DependencyProperty.Register(nameof(IconContent), typeof(object), typeof(IconButton), 
                new PropertyMetadata(null));

        public static readonly DependencyProperty IconFillProperty =
            DependencyProperty.Register(nameof(IconFill), typeof(Brush), typeof(IconButton),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty IconStrokeProperty =
            DependencyProperty.Register(nameof(IconStroke), typeof(Brush), typeof(IconButton),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty IconStrokeThicknessProperty =
            DependencyProperty.Register(nameof(IconStrokeThickness), typeof(double), typeof(IconButton),
                new PropertyMetadata((double)0));

        public static DependencyProperty StackOrientationProperty =
            DependencyProperty.Register(nameof(StackOrientation), typeof(Orientation), typeof(IconButton),
                new PropertyMetadata(Orientation.Horizontal));

        public static DependencyProperty StackFlowDirectionProperty =
            DependencyProperty.Register(nameof(StackFlowDirection), typeof(FlowDirection), typeof(IconButton),
                new PropertyMetadata(FlowDirection.LeftToRight));

        public static readonly DependencyProperty SeparatorMarginProperty =
            DependencyProperty.Register(nameof(SeparatorMargin), typeof(Thickness), typeof(IconButton),
                new PropertyMetadata(new Thickness(2)));

        public static readonly DependencyProperty IconMarginProperty =
            DependencyProperty.Register(nameof(IconMargin), typeof(Thickness), typeof(IconButton),
                new PropertyMetadata(new Thickness(0)));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(IconButton),
                new PropertyMetadata(new CornerRadius(2)));

        public object IconContent 
        { 
            get => (object)GetValue(IconContentProperty); 
            set => SetValue(IconContentProperty, value); 
        }

        public Brush IconFill
        {
            get => (Brush)GetValue(IconFillProperty);
            set => SetValue(IconFillProperty, value);
        }

        public Brush IconStroke
        {
            get => (Brush)GetValue(IconStrokeProperty);
            set => SetValue(IconStrokeProperty, value);
        }

        [TypeConverter(typeof(LengthConverter))]
        public double IconStrokeThickness
        {
            get => (double)GetValue(IconStrokeThicknessProperty);
            set => SetValue(IconStrokeThicknessProperty, value);
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

        public Thickness SeparatorMargin
        {
            get => (Thickness)GetValue(SeparatorMarginProperty);
            set => SetValue(SeparatorMarginProperty, value);
        }

        public Thickness IconMargin
        {
            get => (Thickness)GetValue(IconMarginProperty);
            set => SetValue(IconMarginProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
