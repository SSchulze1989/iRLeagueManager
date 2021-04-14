using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaktionslogik für SelectionControl.xaml
    /// </summary>
    public partial class SelectionControl : UserControl, IModalContent
    {
        public SelectionControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(SelectionControl),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(ICollectionView), typeof(SelectionControl),
                new PropertyMetadata());

        public string Header
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public string Message { get; set; }

        public string SubmitText { get; set; }

        public string CancelText { get; set; }

        public ICollectionView Items
        {
            get => (ICollectionView)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public bool IsLoading { get; private set; }

        public bool CanSubmit()
        {
            return Items.CurrentItem != null;
        }

        public void OnCancel()
        {
            Items.MoveCurrentTo(null);
        }

        public void OnLoad()
        {            
        }

        public Task<bool> OnSubmitAsync()
        {
            return Task.FromResult(true);
        }
    }
}
