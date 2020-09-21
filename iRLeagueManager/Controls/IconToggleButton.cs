using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iRLeagueManager.Controls
{
    public class IconToggleButton : IconButton
    {
        public static DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(IconToggleButton),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        protected override void OnClick()
        {
            IsChecked = !IsChecked;
            OnPropertyChanged(new DependencyPropertyChangedEventArgs(IsCheckedProperty, !IsChecked, IsChecked));
            base.OnClick();
        }
    }
}
