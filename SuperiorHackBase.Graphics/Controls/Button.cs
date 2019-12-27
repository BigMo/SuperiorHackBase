using SuperiorHackBase.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Controls
{
    public class Button : Control
    {
        public event EventHandler<MouseEventExtArgs> Pressed;

        public Button()
        {
            base.MouseUp += (o,e) => Pressed?.Invoke(o, e);
        }

        public override Control AddChild(Control child)
        {
            throw new NotSupportedException();
        }
        public override Control RemoveChild(Control child)
        {
            throw new NotSupportedException();
        }
        public override void Draw(Renderer renderer)
        {
            renderer.FillRoundedRectangle(Rectangle, MouseOver ? ForeColor : BackColor, Math.Min(Width, Height) / 4f);
            renderer.DrawRoundedRectangle(Rectangle, MouseOver ? BackColor : ForeColor, Math.Min(Width, Height) / 4f);
            var size = renderer.MeasureText(Text, Font);
            renderer.DrawText(Text, Font, MouseOver ? BackColor : ForeColor, Rectangle.Center - size / 2f);
        }

        protected override void DrawChildren(Renderer renderer)
        {
            return;
        }
    }
}
