using SuperiorHackBase.Core.Maths;
using SuperiorHackBase.Graphics.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Painting
{
    public interface IControlPaint
    {
        void DrawButton(Rectangle bounds, ControlState state, ButtonState buttonState);
        void DrawCheckBox(Rectangle bounds, ControlState state, CheckState checkState);
        void DrawString(Rectangle bounds, ControlState state, string text);

        void DrawBorder(Rectangle bounds, BorderStyle boderStyle);
    }
}
