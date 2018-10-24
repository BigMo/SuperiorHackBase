using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.Process
{
    public interface IGameModule
    {
        Pointer BaseAddress { get; }
        string Name { get; }
    }
}
