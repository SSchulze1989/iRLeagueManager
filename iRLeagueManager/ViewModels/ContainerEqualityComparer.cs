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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Attributes;

using iRLeagueManager.Models;

namespace iRLeagueManager.ViewModels
{
    public class ContainerModelEqualityComparer<I> : EqualityComparer<I>
    {
        public override bool Equals(I x, I y)
        {
            bool isEqual = true;
            //EqualityCheckPropertyAttributes

            if (x is MappableModel xModel && y is MappableModel yModel)
            {
                return xModel.ModelId.SequenceEqual(yModel.ModelId);
            }

            var EqualityCheckProperties = typeof(I).GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(EqualityCheckPropertyAttribute), true).Count() > 0)
                .ToList();

            if (EqualityCheckProperties.Count() == 0)
            {
                return object.Equals(x, y);
            }

            EqualityCheckProperties.ForEach(p =>
            {
                var xValue = (x != null) ? p.GetValue(x)?.GetHashCode() : null;
                var yValue = (y != null) ? p.GetValue(y)?.GetHashCode() : null;

                if (xValue != yValue || xValue == null || yValue == null)
                {
                    isEqual = false;
                }
            });
            return isEqual;
        }

        public override int GetHashCode(I obj)
        {
            var EqualityCheckProperties = typeof(I).GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(EqualityCheckPropertyAttribute), true).Count() > 0)
                .ToList();
            if (EqualityCheckProperties.Count() > 0)
            {
                int hashCode = 352033288;
                foreach(int propertyHash in EqualityCheckProperties.Select(x => x.GetHashCode()))
                {
                    hashCode = hashCode * -1521134295 + propertyHash;
                }
                return hashCode;
            }
            return base.GetHashCode();
        }
    }
}
