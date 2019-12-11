using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashContainer
{
    public class ValueEqualityComparer : EqualityComparer<InfoStruct>
    {
        public override bool Equals(InfoStruct x, InfoStruct y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode(InfoStruct obj)
        {
            return obj.GetHashCode();
        }
    }
}
