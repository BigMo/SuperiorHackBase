using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SuperiorHackBase.Core.Maths;
using SuperiorHackBase.Graphics.Extensions;
using D2DFactory = SharpDX.Direct2D1.Factory;
using DWriteFactory = SharpDX.DirectWrite.Factory;

namespace SuperiorHackBase.Graphics
{
    public class Renderer
    {
        public D2DFactory D2DFactory { get; private set; }
        public DWriteFactory FontFactory { get; private set; }
        public WindowRenderTarget Device { get; private set; }

        private Dictionary<FontDescription, TextFormat> fonts;
        private Dictionary<BrushDescription, Brush> brushes;
        private Dictionary<int, Vector2[]> circles;

        public Renderer()
        {
            D2DFactory = new D2DFactory();
            FontFactory = new DWriteFactory(SharpDX.DirectWrite.FactoryType.Shared);
            fonts = new Dictionary<FontDescription, TextFormat>();
            brushes = new Dictionary<BrushDescription, Brush>();
            circles = new Dictionary<int, Vector2[]>();
        }

        public void Initialize(IntPtr handle, int width, int height)
        {
            foreach (var font in fonts.Values) font.Dispose();
            fonts.Clear();

            foreach (var brush in brushes.Values) brush.Dispose();
            brushes.Clear();

            if (Device != null) Device.Dispose();

            var hwProps = new HwndRenderTargetProperties();
            hwProps.Hwnd = handle;
            hwProps.PixelSize = new Size2(width, height);
            hwProps.PresentOptions = PresentOptions.Immediately;
            Device = new WindowRenderTarget(D2DFactory, new RenderTargetProperties() { PixelFormat = new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied) }, hwProps);
        }

        private Vector2[] GetCircles(int edges)
        {
            if (circles.ContainsKey(edges)) return circles[edges];
            var vertices = new Vector2[edges];
            var stepSize = 360f / edges;
            for (int i = 0; i < edges; i++)
                vertices[i] = new Vector2(HackMath.Sin(stepSize * i), HackMath.Cos(stepSize * i));
            circles[edges] = vertices;
            return vertices;
        }

        private TextFormat GetFont(FontDescription description)
        {
            if (description == null) return null;
            if (!fonts.ContainsKey(description))
            {
                var font = new TextFormat(FontFactory, description.FontFamily, description.FontWeight, description.FontStyle, description.Size);
                fonts[description] = font;
                return font;
            }
            return fonts[description];
        }

        private Brush GetBrush(BrushDescription description)
        {
            if (description == null) return null;
            if (!brushes.ContainsKey(description))
            {
                var brush = description.CreateBrush(Device);
                brushes[description] = brush;
                return brush;
            }
            return brushes[description];
        }

        public Vector2 MeasureText(string text, FontDescription description)
        {
            var font = GetFont(description);
            if (font == null) throw new Exception();
            Vector2 size;
            using (var layout = new TextLayout(FontFactory, text, font, float.MaxValue, float.MaxValue))
            {
                size = new Vector2(layout.Metrics.Width, layout.Metrics.Height);
            }
            return size;
        }

        public void DrawText(string text, FontDescription font, BrushDescription brush, Vector2 position)
        {
            var size = MeasureText(text, font);
            var _font = GetFont(font);
            var _brush = GetBrush(brush);
            if (_font == null || _brush == null) throw new Exception();
            Device.DrawText(text, _font, new Rectangle(position, size).ToRawRectangleF(), _brush);
        }

        public void DrawLine(Vector2 from, Vector2 to, BrushDescription brush, float width = 1)
        {
            var _brush = GetBrush(brush);
            if (_brush == null) throw new Exception();
            Device.DrawLine(from.ToRawVector(), to.ToRawVector(), _brush, width);
        }

        public void DrawRectangle(Vector2 position, Vector2 size, BrushDescription brush, float width = 1)
        {
            DrawRectangle(new Rectangle(position, size), brush, width);
        }
        public void DrawRectangle(Rectangle rect, BrushDescription brush, float width = 1)
        {
            var _brush = GetBrush(brush);
            if (_brush == null) throw new Exception();
            Device.DrawRectangle(rect.ToRawRectangleF(), _brush, width);
        }

        public void DrawRoundedRectangle(Vector2 position, Vector2 size, BrushDescription brush, float radius, float width = 1)
        {
            DrawRoundedRectangle(new Rectangle(position, size), brush, radius, width);
        }
        
        public void DrawRoundedRectangle(Rectangle rect, BrushDescription brush, float radius, float width = 1)
        {
            var _brush = GetBrush(brush);
            if (_brush == null) throw new Exception();
            Device.DrawRoundedRectangle(new RoundedRectangle()
            {
                Rect = rect.ToRawRectangleF(),
                RadiusX = radius,
                RadiusY = radius
            }, _brush, width);
        }

        public void FillRectangle(Vector2 position, Vector2 size, BrushDescription brush)
        {
            FillRectangle(new Rectangle(position, size), brush);
        }

        public void FillRectangle(Rectangle rect, BrushDescription brush)
        {
            var _brush = GetBrush(brush);
            if (_brush == null) throw new Exception();
            Device.FillRectangle(rect.ToRawRectangleF(), _brush);
        }

        public void FillRoundedRectangle(Vector2 position, Vector2 size, BrushDescription brush, float radius)
        {
            FillRoundedRectangle(new Rectangle(position, size), brush, radius);
        }

        public void FillRoundedRectangle(Rectangle rect, BrushDescription brush, float radius)
        {
            var _brush = GetBrush(brush);
            if (_brush == null) throw new Exception();
            Device.FillRoundedRectangle(new RoundedRectangle()
            {
                Rect = rect.ToRawRectangleF(),
                RadiusX = radius,
                RadiusY = radius
            }, _brush);
        }
    }
}
