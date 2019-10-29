using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Rendering
{
    public interface IGraphicsBuffer : IDisposable
    {
        int Size { get; set; }
        int Pitch { get; set; }
        int Rows { get; set; }
        BufferType Type { get; set; }
        
        void BeginMemoryTransition(int offset, out IntPtr out_data);
        void EndMemoryTransition();


    }
}
