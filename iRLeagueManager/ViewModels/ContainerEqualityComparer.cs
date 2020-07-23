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
