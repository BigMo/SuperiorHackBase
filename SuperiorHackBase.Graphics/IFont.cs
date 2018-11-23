using SuperiorHackBase.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics
{
    public interface IFont
    {
        string Family { get; }
        float Height { get; }
        bool Outlined { get; }
        bool Bold { get; }
        bool Italic { get; }

        Vector2 MeasureString(string text);
    }
}
