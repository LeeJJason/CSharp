using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashContainer
{
    public struct InfoStruct
    {
        public int x;

        public InfoStruct(int x)
        {
            this.x = x;
        }

        public override int GetHashCode()
        {
            return x;
        }

        public bool Equals(InfoStruct infoStruct)
        {
            return this.x == infoStruct.x;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InfoStruct)obj);
        }

        public static bool operator ==(InfoStruct a, InfoStruct b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(InfoStruct a, InfoStruct b)
        {
            return !(a == b);
        }
    }

    public class InfoClass
    {
        public int x;
        public InfoClass(int x)
        {
            this.x = x;
        }

        public override int GetHashCode()
        {
            return this.x;
        }

        public bool Equals(InfoClass obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            return this.x == obj.x;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InfoClass)obj);
        }

        public static bool operator ==(InfoClass a, InfoClass b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(null, a)) return false;
            return a.Equals(b);
        }

        public static bool operator !=(InfoClass a, InfoClass b)
        {
            return !(a == b);
        }
    }
}
