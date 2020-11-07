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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace iRLeagueDatabase.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Iterate throug a string of nested properties and returns a NestedPropertyInfo from which the
        /// property value can be directly read.
        /// </summary>
        /// <param name="propertyName">Nested property string. Example: Door.Handle.Color</param>
        /// <returns>NestedPropertyInfo for the nested property. Example: Use .GetValue(CarObject) to get color of door from Door.Handle.Color</returns>
        public static PropertyInfo GetNestedPropertyInfo(this Type type, string propertyName)
        {
            // split property name into single parts
            var parts = propertyName.Split('.');
            if (parts.Length == 0)
            {
                return null;
            }

            // Start iteration from firts part
            PropertyInfo currentProperty = type.GetProperty(parts.First());
            if (currentProperty == null)
            {
                return null;
            }

            // Iterate over remaining parts and create NestedPropertyInfo for each part
            foreach(var name in parts.Skip(1))
            {
                var currentPropertyType = currentProperty.PropertyType;
                var childProperty = currentPropertyType.GetProperty(name);

                if (childProperty == null)
                {
                    return null;
                }

                currentProperty = new NestedPropertyInfo(childProperty, currentProperty);
            }

            return currentProperty;
        }

        //public static object GetNestedPropertyValue(this Type type, object obj, IEnumerable<PropertyInfo> nestedProperties)
        //{
        //    if (nestedProperties.Count() == 0)
        //    {
        //        return obj;
        //    }
        //    else
        //    {
        //        var currentPropertyValue = nestedProperties.First().GetValue(obj);
        //        nestedProperties = nestedProperties.Skip(1);
        //        return type.GetNestedPropertyValue(currentPropertyValue, nestedProperties);
        //    }
        //}
    }
}
