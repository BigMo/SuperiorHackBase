using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics
{
    public struct Color4f
    {
        public float R, G, B, A;

        public int ARGB
        {
            get
            {
                return
                  (int)(255f * A) << 24 |
                  (int)(255f * R) << 16 |
                  (int)(255f * G) << 8 |
                  (int)(255f * B) << 0;
            }
        }

        public int RGBA
        {
            get
            {
                return
                  (int)(255f * R) << 24 |
                  (int)(255f * G) << 16 |
                  (int)(255f * B) << 8 |
                  (int)(255f * A) << 0;
            }
        }

        public Color4f(byte r, byte g, byte b) : this(r, g, b, (float)byte.MaxValue) { }
        public Color4f(byte r, byte g, byte b, byte a) : this((float)r / (float)byte.MaxValue, (float)g / (float)byte.MaxValue, (float)b / (float)byte.MaxValue, (float)a / (float)byte.MaxValue) { }
        public Color4f(float r, float g, float b) : this(r, g, b, 1) { }
        public Color4f(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static Color4f FromRGBA(int rgba)
        {
            return new Color4f(
                (byte)((rgba & 0xFF000000) >> 24),
                (byte)((rgba & 0x00FF0000) >> 16),
                (byte)((rgba & 0x0000FF00) >> 8),
                (byte)((rgba & 0x000000FF) >> 0));
        }

        public static Color4f FromARGB(int argb)
        {
            return new Color4f(
                (byte)((argb & 0x00FF0000) >> 16),
                (byte)((argb & 0x0000FF00) >> 8),
                (byte)((argb & 0x000000FF) >> 0),
                (byte)((argb & 0xFF000000) >> 24));
        }
    }
}
