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

using iRLeagueManager;
using iRLeagueManager.Controls;
using iRLeagueManager.Converters;
using iRLeagueManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace iRLeagueManager.Controls
{
    public class FilterValueSelectTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            var parent = element.Parent;

            if (element != null && item != null && item is ResultsFilterOptionViewModel resultsFilterOption)
            {
                if (resultsFilterOption.Comparator == Enums.ComparatorTypeEnum.InList)
                {
                    return element.FindResource("FilterValueListSelection") as DataTemplate;
                }
                else
                {
                    return element.FindResource("FilterValueEdit") as DataTemplate;
                }
            }
            else if (element != null && item != null && item is StandingsFilterOptionViewModel standingsFilterOption)
            {
                if (standingsFilterOption.Comparator == Enums.ComparatorTypeEnum.InList)
                {
                    return element.FindResource("FilterValueListSelection") as DataTemplate;
                }
                else
                {
                    return element.FindResource("FilterValueEdit") as DataTemplate;
                }
            }
            return null;
        }
    }
}
