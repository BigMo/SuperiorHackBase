using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics
{
    public class FontDescription
    {
#if DEBUG
        public static FontDescription DEBUG { get; private set; } = new FontDescription("Courier New", 14f);
#endif
        public string FontFamily { get; private set; }
        public FontWeight FontWeight { get; private set; }
        public FontStyle FontStyle { get; private set; }
        public float Size { get; private set; }

        public FontDescription(string fontFamily, float size, FontWeight weight = FontWeight.Regular, FontStyle style = FontStyle.Normal)
        {
            FontFamily = fontFamily;
            Size = size;
            FontWeight = weight;
            FontStyle = style;
        }

        public override bool Equals(object obj)
        {
            return obj is FontDescription description &&
                   FontFamily == description.FontFamily &&
                   FontWeight == description.FontWeight &&
                   FontStyle == description.FontStyle &&
                   Size == description.Size;
        }

        public override int GetHashCode()
        {
            var hashCode = 577408457;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FontFamily);
            hashCode = hashCode * -1521134295 + FontWeight.GetHashCode();
            hashCode = hashCode * -1521134295 + FontStyle.GetHashCode();
            hashCode = hashCode * -1521134295 + Size.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(FontDescription left, FontDescription right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FontDescription left, FontDescription right)
        {
            return !(left == right);
        }
    }
}
