using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics
{
    public struct Color
    {
        public static Color Black { get; private set; } = new Color(0f, 0f, 0f, 1f);
        public static Color White { get; private set; } = new Color(1f, 1f, 1f, 1f);
        public static Color Red { get; private set; } = new Color(1f, 0f, 0f, 1f);
        public static Color Green { get; private set; } = new Color(0f, 1f, 0f, 1f);
        public static Color Blue { get; private set; } = new Color(0f, 0f, 1f, 1f);
        public static Color Transparent { get; private set; } = new Color(0f, 0f, 0f, 0f);

        public float R, G, B, A;

        public Color(Color c, int a) : this(c.R, c.G, c.B, a / 255f) { }
        public Color(Color c, float a) : this(c.R, c.G, c.B, a) { }
        public Color(Color c) : this(c.R, c.G, c.B, c.A) { }
        public Color(int r, int g, int b) : this(r, g, b, 255) { }
        public Color(int r, int g, int b, int a) : this(r / 255f, g / 255f, b / 255f, a / 255f) { }
        public Color(float r, float g, float b) : this(r, g, b, 1f) { }
        public Color(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color Lerp(Color to, float s)
        {
            return new Color(
                R + (to.R - R) * s,
                G + (to.G - G) * s,
                B + (to.B - B) * s,
                A + (to.A - A) * s);
        }

        public int ToRGBA()
        {
            return
                ((int)(255 * R) << 24) |
                ((int)(255 * G) << 16) |
                ((int)(255 * B) << 8) |
                ((int)(255 * A));
        }
        public int ToARGB()
        {
            return
                ((int)(255 * A) << 24) |
                ((int)(255 * R) << 16) |
                ((int)(255 * G) << 8) |
                ((int)(255 * B));
        }
    }
}
