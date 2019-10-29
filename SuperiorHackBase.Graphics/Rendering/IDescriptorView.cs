using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Rendering
{
    public interface IDescriptorView : IDisposable
    {
        IGraphicsDevice Device { get; set; }

        IGraphicsBuffer Buffer { get; set; }
        DescriptorViewType Type { get; set; }
    }
}
