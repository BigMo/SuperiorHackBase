using SuperiorHackBase.Core.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.Process
{
    public interface IGameProcess
    {
        int PID { get; }
        string Name { get; }
        bool IsRunning { get; }
        bool InForeground { get; }
        IEnumerable<IGameModule> Modules { get; }
        IEnumerable<WinAPI.MEMORY_BASIC_INFORMATION> Pages { get; }

        void UpdatePages();
        Task<bool> Execute(Pointer function, Pointer parameter);
        IMemory CreateMemoryInterface();
    }
}
