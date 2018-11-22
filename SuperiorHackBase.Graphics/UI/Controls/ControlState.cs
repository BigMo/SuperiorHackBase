using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Controls
{
    [Flags]
    public enum ControlState
    {
        Normal,
        Disabled,
        Focused,
        Pressed
    }
}
