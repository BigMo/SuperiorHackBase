using SuperiorHackBase.Core.ProcessInteraction.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Process
{
    public interface IProcess
    {
        int PID { get; }
        string Name { get; }
        bool IsRunning { get; }
        bool InForeground { get; }
        IEnumerable<IModule> Modules { get; }
        IEnumerable<WinAPI.MEMORY_BASIC_INFORMATION> Pages { get; }

        void UpdatePages();
        Task<bool> Execute(Pointer function, Pointer parameter);
        IMemory CreateMemoryInterface();
    }
}
