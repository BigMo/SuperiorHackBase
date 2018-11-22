using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Controls
{
    [Flags]
    public enum ButtonState
    {
        Normal = 0,
        Pressed = 1 << 1
    }
}
