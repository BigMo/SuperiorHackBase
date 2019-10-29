using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Rendering
{
    public interface ICommandList : IDisposable
    {
        IGraphicsDevice GraphicsDevice { get; set; }
        IRenderProgram RenderProgram { get; set; }
        IDescriptorView RenderTarget { get; set; }
        IGraphicsBuffer VertexBuffer { get; set; }
        IGraphicsBuffer IndexBuffer { get; set; }
        IGraphicsBuffer VertexShaderInfo { get; set; }
        IGraphicsBuffer PixelShaderInfo { get; set; }

        void BeginRecord(bool immediate = false);
        void EndRecord();

        void BindShaderInfo(IGraphicsBuffer graphicsBuffer);
        void BindVertexBuffer(IGraphicsDevice graphicsBuffer);
        void BindIndexBuffer(IGraphicsBuffer graphicsBuffer);
        void DrawPrimitive(int vertexOffset, int vertexCount);
        void DrawIndexedPrimitive(int vertexOffset, int vertexCount, int indexOffset, int indexCount);

    }
}
