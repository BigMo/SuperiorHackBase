using SuperiorHackBase.Core.ProcessInteraction.Memory;
using SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns;
using SuperiorHackBase.Core.ProcessInteraction.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction
{
    public interface IHackContext
    {
        IProcess Process { get; }
        IMemory Memory { get; }

        ScanResult Scan(Pattern pattern);
    }
}
