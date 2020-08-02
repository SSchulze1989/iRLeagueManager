using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iRLeagueManager.Views
{
    /// <summary>
    /// Interaktionslogik für SessionSelectControl.xaml
    /// </summary>
    public partial class SessionSelectControl : UserControl
    {
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(SessionSelectControl),
                new PropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public SessionSelectControl()
        {
            InitializeComponent();

            // Set default Item template
            //var defaultItemTemplate = TryFindResource("DefaultItemTemplate") as DataTemplate;
            //if (defaultItemTemplate != null && ItemTemplate == null)
            //    ItemTemplate = defaultItemTemplate;
        }
    }
}
