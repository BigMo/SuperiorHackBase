using SuperiorHackBase.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Controls.Layouts
{
    public class FlexLayout : Layout
    {
        public override void RearrangeControls()
        {
            var pos = new Vector2(Padding.Left, Padding.Top);
            var firstControl = true;
            var newPos = pos;
            var maxHeight = 0f;

            foreach (var c in Children)
            {
                maxHeight = Math.Max(maxHeight, c.Height);
                if (!firstControl)
                {
                    if (pos.X + ChildMargins.Left + c.Width > Width) //Width overflow?
                    {
                        pos.Y += maxHeight + ChildMargins.Bottom; //Move to next line
                        pos.X = Padding.Left;
                        maxHeight = 0;
                    }
                }
                else
                {
                    firstControl = false;
                }
                c.Location = pos;
                pos.X += ChildMargins.Right + c.Width;
            }
        }
    }
}
