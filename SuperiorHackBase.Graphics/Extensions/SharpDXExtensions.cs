using SharpDX.Mathematics.Interop;
using SuperiorHackBase.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Extensions
{
    public static class SharpDXExtensions
    {
        public static RawVector2 ToRawVector(this Vector2 vec)
        {
            return new RawVector2(vec.X, vec.Y);
        }
        public static RawVector3 ToRawVector(this Vector3 vec)
        {
            return new RawVector3(vec.X, vec.Y, vec.Z);
        }
        public static RawRectangleF ToRawRectangleF(this Rectangle rect)
        {
            return new RawRectangleF(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
        public static RawRectangle ToRawRectangle(this Rectangle rect)
        {
            return new RawRectangle((int)rect.Left, (int)rect.Top, (int)rect.Right, (int)rect.Bottom);
        }
        public static RawColor4 ToRawColor(this Color color)
        {
            return new RawColor4(color.R, color.G, color.B, color.A);
        }
        public static Color ToColor(this RawColor4 color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
