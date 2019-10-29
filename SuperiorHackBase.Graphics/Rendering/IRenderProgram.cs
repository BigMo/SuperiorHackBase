using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Rendering
{
    public interface IRenderProgram : IDisposable
    {
        IGraphicsDevice Device { get; set; }
        
        void Cook();
    }
}
