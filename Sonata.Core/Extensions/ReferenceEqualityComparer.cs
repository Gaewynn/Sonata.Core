using System.Collections.Generic;

namespace Sonata.Core.Extensions
{
    internal class ReferenceEqualityComparer : EqualityComparer<object>
    {
        #region Methods

        #region EqualityComparer<object> Members

        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }

        public override int GetHashCode(object obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }  

        #endregion

        #endregion
    }
}
