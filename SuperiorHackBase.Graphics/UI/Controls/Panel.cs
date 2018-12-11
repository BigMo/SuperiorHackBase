using SuperiorHackBase.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Controls
{
    public class Panel : Control
    {
        public override void Draw(IRenderer renderer)
        {
            if (renderer == null)
                return;

            var absPos = AbsolutePosition;
            var bounds = new Rectangle(absPos.X, absPos.Y, Width, Height);
            if (DrawBackground)
                renderer.FillRectangle(bounds, BackgroundColor);
            if (DrawBorder)
                renderer.DrawRectangle(bounds, BorderColor, 1f);

            foreach (var child in Children)
                child.Draw(renderer);
        }
    }
}
