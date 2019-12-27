using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Controls
{
    public class Label : Control
    {
        public bool AutoSize { get; set; }
        public bool DrawBorder { get; set; }

        public Label() : base()
        {
            AutoSize = true;
            DrawBorder = false;
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
            if (string.IsNullOrEmpty(Text)) return;
            if (AutoSize)
                Size = renderer.MeasureText(Text, Font);

            renderer.FillRectangle(Rectangle, BackColor);
            if (DrawBorder) renderer.DrawRectangle(Rectangle, ForeColor);
            var size = renderer.MeasureText(Text, Font);
            renderer.DrawText(Text, Font, ForeColor, Rectangle.Center - size / 2f);
        }

        protected override void DrawChildren(Renderer renderer)
        {
            return;
        }
    }
}
