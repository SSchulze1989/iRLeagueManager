using iRLeagueManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace iRLeagueManager.Extensions
{
    public static class ModalExtensions
    {
        public static bool IsValid(this DependencyObject dependencyObject)
        {
            // The dependency object is valid if it has no errors and all
            // of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(dependencyObject) &&
            LogicalTreeHelper.GetChildren(dependencyObject)
            .OfType<DependencyObject>()
            .All(IsValid);
    }
    }
}
