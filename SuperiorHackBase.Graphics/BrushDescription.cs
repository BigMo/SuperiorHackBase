using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using SuperiorHackBase.Core.Maths;
using SuperiorHackBase.Graphics.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics
{
    public abstract class BrushDescription
    {
        public static BrushDescription Black { get; private set; } = new SolidBrushDescription(Color.Black);
        public static BrushDescription White { get; private set; } = new SolidBrushDescription(Color.White);
        public static BrushDescription Red { get; private set; } = new SolidBrushDescription(Color.Red);
        public static BrushDescription Green { get; private set; } = new SolidBrushDescription(Color.Green);
        public static BrushDescription Blue { get; private set; } = new SolidBrushDescription(Color.Blue);
        public static BrushDescription Transparent { get; private set; } = new SolidBrushDescription(Color.Transparent);

        public abstract Brush CreateBrush(WindowRenderTarget device);

        public static BrushDescription CreateSolidBrush(Color color)
        {
            return new SolidBrushDescription(color);
        }
        public static BrushDescription CreateLinearBrush(Vector2 pFrom, Vector2 pTo, Color cFrom, Color cTo)
        {
            return new LinearGradientBrushDescription(pFrom, pTo, cFrom, cTo);
        }

        private class SolidBrushDescription : BrushDescription
        {
            private Color color;
            public SolidBrushDescription(Color color) { this.color = color; }
            public override Brush CreateBrush(WindowRenderTarget device)
            {
                return new SolidColorBrush(device, color.ToRawColor());
            }
        }
        private class LinearGradientBrushDescription : BrushDescription
        {
            private Vector2 pFrom, pTo;
            private Color cFrom, cTo;
            public LinearGradientBrushDescription(Vector2 pFrom, Vector2 pTo, Color cFrom, Color cTo)
            {
                this.pFrom = pFrom;
                this.pTo = pTo;
                this.cFrom = cFrom;
                this.cTo = cTo;
            }
            public override Brush CreateBrush(WindowRenderTarget device)
            {
                return new LinearGradientBrush(device,
                    new LinearGradientBrushProperties()
                    {
                        StartPoint = pFrom.ToRawVector(),
                        EndPoint = pTo.ToRawVector()
                    },
                    new GradientStopCollection(device,
                        new GradientStop[]{
                            new GradientStop(){ Position = 0f, Color = cFrom.ToRawColor() },
                            new GradientStop(){ Position = 1f, Color = cTo.ToRawColor() }
                        }));
            }
        }
    }
}
