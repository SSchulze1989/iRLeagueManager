using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Attributes
{
    /// <summary>
    /// Attribute for checking equality when using ContainerEqualityComparer
    /// </summary>
    public class EqualityCheckPropertyAttribute : Attribute
    {
        bool CheckProperty { get; set; }
        public EqualityCheckPropertyAttribute(bool checkProperty = true)
        {
            CheckProperty = checkProperty;
        }
    }
}
