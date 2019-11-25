using SuperiorHackBase.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Controls.Layouts
{
    public class VerticalLayout : Layout
    {
        public override void RearrangeControls()
        {
            var pos = new Vector2(Padding.Left, Padding.Top);
            var firstControl = true;
            foreach (var c in Children)
            {
                if (!firstControl)
                    pos.Y += ChildMargins.Top;
                else
                    firstControl = false;

                c.Location = pos;
                pos.Y += c.Height + ChildMargins.Bottom;
            }
        }
    }
}
