using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperiorHackBase.Core.Maths;
using SuperiorHackBase.Graphics.UI.Painting;

namespace SuperiorHackBase.Graphics.UI.Controls
{
    public class Label : Control
    {
        public Label()
        {
            AutoSize = true;
        }

        public override void Draw(IControlPaint controlPaint)
        {
            if (controlPaint == null)
                return;
            //TODO: Draw *duh*
            //Draw background
            //Draw text
        }

        protected override Vector2 CalculateSize()
        {
            if (Text == null || Font == null)
                return Vector2.Zero;

            return Font.MeasureString(Text);
        }
    }
}
