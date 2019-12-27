using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Controls.Layouts
{
    public struct Distance
    {
        public float Left, Top, Right, Bottom;

        public Distance(float any = 0) : this(any, any, any, any) { }
        public Distance(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public override bool Equals(object obj)
        {
            return obj is Distance distance &&
                   Left == distance.Left &&
                   Top == distance.Top &&
                   Right == distance.Right &&
                   Bottom == distance.Bottom;
        }

        public override int GetHashCode()
        {
            var hashCode = -1819631549;
            hashCode = hashCode * -1521134295 + Left.GetHashCode();
            hashCode = hashCode * -1521134295 + Top.GetHashCode();
            hashCode = hashCode * -1521134295 + Right.GetHashCode();
            hashCode = hashCode * -1521134295 + Bottom.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Distance left, Distance right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Distance left, Distance right)
        {
            return !(left == right);
        }
    }
}
