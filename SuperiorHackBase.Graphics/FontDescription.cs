using SuperiorHackBase.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics
{
    public class FontDescription : IComparable
    {
        public string Family { get; }
        public float Height { get; }
        public bool Outlined { get; }
        public bool Bold { get; }
        public bool Italic { get; }

        public FontDescription(string family, float height, bool outlined = false, bool bold = false, bool italic = false)
        {
            Family = family;
            Height = height;
            Outlined = outlined;
            Bold = bold;
            Italic = italic;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is FontDescription))
                return false;
            var desc = (FontDescription)obj;

            return desc.Family == Family && desc.Height == Height && desc.Outlined == Outlined && desc.Bold == Bold && desc.Italic == Italic;
        }

        public override int GetHashCode()
        {
            return Family.GetHashCode() ^ Height.GetHashCode() << 8 ^ Outlined.GetHashCode() << 16 ^ Bold.GetHashCode() << 24 ^ Italic.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is FontDescription))
                return 1;

            int val = 0;
            var desc = (FontDescription)obj;
            if ((val = desc.Family.CompareTo(Family)) != 0)
                return val;
            if ((val = desc.Height.CompareTo(Height)) != 0)
                return val;
            if ((val = desc.Outlined.CompareTo(Outlined)) != 0)
                return val;
            if ((val = desc.Bold.CompareTo(Bold)) != 0)
                return val;
            if ((val = desc.Italic.CompareTo(Italic)) != 0)
                return val;

            return 0;
        }
    }
}
