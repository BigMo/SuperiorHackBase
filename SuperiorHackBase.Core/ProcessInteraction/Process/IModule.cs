using SuperiorHackBase.Core.ProcessInteraction.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Process
{
    public interface IModule
    {
        Pointer BaseAddress { get; }
        int Size { get; }
        string Name { get; }
    }
}
