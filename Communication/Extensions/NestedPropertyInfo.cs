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
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.Extensions
{
    /// <summary>
    /// Provides info on a property that is nested behind one or more other properties.
    /// Example: Car.Door.Handle.Color
    /// </summary>
    public class NestedPropertyInfo : PropertyInfo
    {
        public PropertyInfo ParentProperty { get; }

        public PropertyInfo Property { get; }

        public override Type PropertyType => Property.PropertyType;

        public override PropertyAttributes Attributes => Property.Attributes;

        public override bool CanRead => Property.CanRead;

        public override bool CanWrite => Property.CanWrite;

        public override string Name => Property.Name;

        public override Type DeclaringType => Property.DeclaringType;

        public override Type ReflectedType => Property.ReflectedType;

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            return Property.GetAccessors(nonPublic);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return Property.GetCustomAttributes(inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return Property.GetCustomAttributes(attributeType, inherit);
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return Property.GetGetMethod(nonPublic);
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            return Property.GetIndexParameters();
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return Property.GetSetMethod(nonPublic);
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            var parentObject = ParentProperty.GetValue(obj, invokeAttr, binder, index, culture);
            if (parentObject == null)
            {
                return null;
            }
            return Property.GetValue(parentObject, invokeAttr, binder, index, culture);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return Property.IsDefined(attributeType, inherit);
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            var parentObject = ParentProperty.GetValue(obj, invokeAttr, binder, index, culture);
            if (parentObject == null)
            {
                throw new ArgumentNullException("Could not set nested property value: Parent object was null");
            }
            Property.SetValue(ParentProperty.GetValue(obj, invokeAttr, binder, index, culture), value, invokeAttr, binder, index, culture);
        }

        public NestedPropertyInfo(PropertyInfo property, PropertyInfo parentProperty)
        {
            Property = property;
            ParentProperty = parentProperty;
        }

        public override string ToString()
        {
            return $"{ParentProperty}.{Property.Name}";
        }
    }
}
