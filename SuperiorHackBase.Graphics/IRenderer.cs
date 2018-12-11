using SuperiorHackBase.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics
{
    public interface IRenderer : IDisposable
    {
        bool Initialized { get; }
        void StartFrame();
        void EndFrame();

        void Reset(int width, int height);
        void Initialize(IntPtr hWnd, int width, int height);
        void Destroy();

        void DrawLine(Vector2 from, Vector2 to, Color4f color, float thickness);
        void DrawRectangle(Rectangle rect, Color4f color, float thickness);
        void DrawCircle(Rectangle rect, Color4f color, int numElements, float thickness);
        void DrawString(Rectangle rect, string text, FontDescription font, Color4f color);

        void FillRectangle(Rectangle rect, Color4f color);
        void FillCircle(Rectangle rect, Color4f color, int numElements);

        Vector2 MeasureString(string text, FontDescription font);
    }
}
