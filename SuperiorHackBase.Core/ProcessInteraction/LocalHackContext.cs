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
    public class LocalHackContext : IHackContext
    {
        public IProcess Process { get; private set; }
        public IMemory Memory { get; private set; }

        public static IHackContext CreateContext(string processName, WinAPI.ProcessAccessFlags flags = WinAPI.ProcessAccessFlags.All)
        {
            var processes = System.Diagnostics.Process.GetProcessesByName(processName);
            if (processes.Length == 0)
                throw new Exception($"Could not find process \"{processName}\"");
            if (processes.Length > 1)
                throw new Exception($"There are multiple instances of \"{processName}\"");
            return CreateContext(processes[0], flags);
        }

        public static IHackContext CreateContext(int pid, WinAPI.ProcessAccessFlags flags = WinAPI.ProcessAccessFlags.All)
        {
            var process = System.Diagnostics.Process.GetProcessById(pid);
            if (process == null)
                throw new Exception($"Could not find process by id {pid}");

            return CreateContext(process, flags);
        }

        public static IHackContext CreateContext(System.Diagnostics.Process process, WinAPI.ProcessAccessFlags flags = WinAPI.ProcessAccessFlags.All)
        {
            var proc = new LocalProcess(process);
            var mem = proc.CreateMemoryInterface(flags, true);

            return new LocalHackContext(proc, mem);
        }

        public ScanResult Scan(Pattern pattern)
        {
            var result = pattern.Find(this);
            return result;
        }

        private LocalHackContext(IProcess proc, IMemory mem)
        {
            Process = proc;
            Memory = mem;
        }
    }
}
