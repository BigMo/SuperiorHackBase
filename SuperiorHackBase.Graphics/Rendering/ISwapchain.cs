using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Rendering
{
    public interface ISwapchain : IDisposable
    {
        IDescriptorView[] Buffers { get; set; }

        void ResizeBuffers(int width, int height);
        void Present(bool immediate);
    }
}
