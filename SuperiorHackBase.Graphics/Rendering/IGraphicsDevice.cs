using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Rendering
{
    public interface IGraphicsDevice : IDisposable
    {
        void CreateVertexBuffer(AttributeHost attributeHost, int maxVertices);
        void CreateVertexBuffer(int size);

        void CreateIndexBuffer(Format format, int maxIndices);
        void CreateIndexBuffer(int size);

        void CreateTexture(Format format, int width, int height);

        void CreateDescriptorView(IGraphicsBuffer graphicsBuffer, DescriptorViewType type);

        ICommandList CreateCommandList();

        void ExecuteCommandList(ICommandList commandList);
    }
}
