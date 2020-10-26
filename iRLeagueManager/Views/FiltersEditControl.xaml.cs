using iRLeagueManager.Extensions;
using iRLeagueManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
    /// Interaktionslogik für FiltersEditControl.xaml
    /// </summary>
    public partial class FiltersEditControl : UserControl, IModalContent
    {
        public FilterEditViewModel ViewModel => DataContext as FilterEditViewModel;

        public string Header { get; set; } = "Edit results Filters";

        public string SubmitText { get; set; } = "Save";

        public string CancelText { get; set; } = "Cancel";

        public bool IsLoading { get; private set; } = false;

        public FiltersEditControl()
        {
            DataContextChanged += FiltersEditControl_DataContextChanged;
            InitializeComponent();
        }

        private void FiltersEditControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.ViewOpenActionDialog += ViewModel_ViewOpenActionDialog;
            }
        }

        private void ViewModel_ViewOpenActionDialog(FilterEditViewModel sender, string title, string message, Action<FilterEditViewModel> okAction)
        {
            if (MessageBox.Show(message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                okAction?.Invoke(sender);
            }
        }

        public bool CanSubmit()
        {
            return true;
        }

        public void OnCancel()
        {
            return;
        }

        public void OnLoad()
        {
            return;
        }

        public async Task<bool> OnSubmitAsync()
        {
            if (ViewModel == null || this.IsValid() == false)
            {
                return false;
            }
            await ViewModel.SaveChanges();
            return true;
        }

        private void Comparator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.Tag is ContentControl contentControl)
            {
                contentControl.InvalidateProperty(ContentTemplateSelectorProperty);
            }
        }
    }
}
