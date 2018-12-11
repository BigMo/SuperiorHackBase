using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using FontFactory = SharpDX.DirectWrite.Factory;
using Factory = SharpDX.Direct2D1.Factory;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using RawColor4 = SharpDX.Mathematics.Interop.RawColor4;
using RawVector2 = SharpDX.Mathematics.Interop.RawVector2;
using RawRectangleF = SharpDX.Mathematics.Interop.RawRectangleF;
using SharpDX.DXGI;

using SuperiorHackBase.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperiorHackBase.Core.Maths;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace SuperiorHackBase.Rendering.SharpDX.D2D1
{
    public class D2D1Renderer : IRenderer
    {
        #region VARIABLES
        private HwndRenderTargetProperties renderTargetProperties;
        private WindowRenderTarget device;
        private FontFactory fontFactory;
        private Factory factory;
        private Dictionary<FontDescription, TextFormat> fonts;
        private Dictionary<int, RawVector2[]> circleElements;
        private bool frameStarted = false;
        #endregion

        #region PROPERTIES
        public WindowRenderTarget Device => device;
        public Color4f BackgroundColor { get; set; } = new Color4f(0, 0, 0, 0);
        public bool Initialized { get; private set; }
        #endregion

        #region DEVICE
        public void Initialize(IntPtr hWnd, int width, int height)
        {
            factory = new Factory();
            fontFactory = new FontFactory();
            fonts = new Dictionary<FontDescription, TextFormat>();
            circleElements = new Dictionary<int, RawVector2[]>();
            renderTargetProperties = new HwndRenderTargetProperties()
            {
                Hwnd = hWnd,
                PixelSize = new Size2(width, height),
                PresentOptions = PresentOptions.None
            };
            Reset(width, height);
            Initialized = true;
        }

        public void Destroy()
        {
            Dispose(false);
        }

        public Vector2 MeasureString(string text, FontDescription font)
        {
            using (var layout = new TextLayout(fontFactory, text, GetCreateFont(font), float.MaxValue, float.MaxValue))
                return new Vector2(layout.Metrics.Width, layout.Metrics.Height);
        }

        public void Reset(int width, int height)
        {
            renderTargetProperties.PixelSize = new Size2(width, height);
            device = new WindowRenderTarget(factory, new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), renderTargetProperties);
            device.TextAntialiasMode = TextAntialiasMode.Cleartype;
        }

        public void StartFrame()
        {
            if (frameStarted)
                return;
            Debug.WriteLine("[RND] StartFrame()");
            device.BeginDraw();
            device.Clear(ConvertColor(BackgroundColor));
            frameStarted = true;
        }

        public void EndFrame()
        {
            if (!frameStarted)
                return;
            Debug.WriteLine("[RND] EndFrame()");
            device.EndDraw();
            frameStarted = false;
        }

        private TextFormat GetCreateFont(FontDescription description)
        {
            if (!fonts.ContainsKey(description))
            {
                return CreateFont(description);
            }
            return fonts[description];
        }
        private TextFormat CreateFont(FontDescription description)
        {
            var style = description.Italic ? FontStyle.Italic : FontStyle.Normal;
            var weight = description.Bold ? FontWeight.Bold : FontWeight.Normal;
            var font = new TextFormat(fontFactory, description.Family, weight, style, description.Height);
            return fonts[description] = font;
        }
        private RawVector2[] GetCreateCircle(int numElements)
        {
            if (circleElements.ContainsKey(numElements))
                return circleElements[numElements];

            RawVector2[] elements = new RawVector2[numElements];
            float stepSize = 360f / numElements;
            for (int i = 0; i < numElements; i++)
                elements[i] = new RawVector2(
                    HackMath.RadiansToDegrees(Math.Sin(HackMath.DegreesToRadians(i * stepSize))),
                    HackMath.RadiansToDegrees(Math.Cos(HackMath.DegreesToRadians(i * stepSize))));

            circleElements[numElements] = elements;
            return elements;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RawColor4 ConvertColor(Color4f color)
        {
            return new RawColor4(color.R, color.G, color.B, color.A);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RawVector2 ConvertVector(Vector2 vector)
        {
            return new RawVector2(vector.X, vector.Y);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RawRectangleF ConvertRect(Rectangle rect)
        {
            return new RawRectangleF(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Ellipse ConvertEllipse(Rectangle rect)
        {
            return new Ellipse(ConvertVector(rect.Center), rect.Width / 2f, rect.Height / 2f);
        }
        #endregion

        #region DRAWING
        public void DrawCircle(Rectangle rect, Color4f color, int numElements, float thickness)
        {
            using (SolidColorBrush brush = new SolidColorBrush(device, ConvertColor(color)))
                device.DrawEllipse(ConvertEllipse(rect), brush);
        }

        public void DrawLine(Vector2 from, Vector2 to, Color4f color, float thickness)
        {
            using (SolidColorBrush brush = new SolidColorBrush(device, ConvertColor(color)))
                device.DrawLine(ConvertVector(from), ConvertVector(to), brush, thickness);
        }

        public void DrawRectangle(Rectangle rect, Color4f color, float thickness)
        {
            using (SolidColorBrush brush = new SolidColorBrush(device, ConvertColor(color)))
                device.DrawRectangle(ConvertRect(rect), brush);
        }

        public void DrawString(Rectangle rect, string text, FontDescription font, Color4f color)
        {
            using (SolidColorBrush brush = new SolidColorBrush(device, ConvertColor(color)))
                device.DrawText(text, GetCreateFont(font), ConvertRect(rect), brush, DrawTextOptions.Clip | DrawTextOptions.NoSnap);
        }

        public void FillCircle(Rectangle rect, Color4f color, int numElements)
        {
            using (SolidColorBrush brush = new SolidColorBrush(device, ConvertColor(color)))
                device.FillEllipse(ConvertEllipse(rect), brush);
        }

        public void FillRectangle(Rectangle rect, Color4f color)
        {
            using (SolidColorBrush brush = new SolidColorBrush(device, ConvertColor(color)))
                device.FillRectangle(ConvertRect(rect), brush);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Initialized = false;
                foreach (var font in fonts.Values.ToArray())
                    font.Dispose();
                factory.Dispose();
                fontFactory.Dispose();
                device.Dispose();
                device = null;

                disposedValue = true;
            }
        }

        ~D2D1Renderer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        #endregion
    }
}
