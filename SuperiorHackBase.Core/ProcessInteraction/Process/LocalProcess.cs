using SuperiorHackBase.Core.ProcessInteraction.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Process
{
    public class LocalProcess : IProcess
    {
        protected System.Diagnostics.Process Process { get; private set; }
        private IEnumerable<WinAPI.MEMORY_BASIC_INFORMATION> pages;
        private IntPtr execHandle;

        public IntPtr MainWindow => Process.MainWindowHandle;
        public bool IsRunning => !Process.HasExited;
        public bool InForeground => IsRunning && WinAPI.GetForegroundWindow() == Process.MainWindowHandle;
        public int PID => Process.Id;
        public string Name => Process.ProcessName;
        public IEnumerable<IModule> Modules { get { return Process.Modules.Cast<ProcessModule>().Select(x => new LocalModule(x)); } }
        public IEnumerable<WinAPI.MEMORY_BASIC_INFORMATION> Pages
        {
            get
            {
                if (pages == null || !pages.Any())
                    UpdatePages();
                return pages;
            }
        }

        public LocalProcess(System.Diagnostics.Process proc)
        {
            Process = proc ?? throw new ArgumentException("Process must not be null");
            if (proc.HasExited)
                throw new ArgumentException("Process has exited");

            execHandle = WinAPI.OpenProcess(WinAPI.ProcessAccessFlags.CreateThread, false, proc.Id);
            if (execHandle == IntPtr.Zero)
                throw new Exception("Failed to aquire CRT handle", new Win32Exception(Marshal.GetLastWin32Error()));
        }

        public static LocalProcess Find(string name)
        {
            var procs = System.Diagnostics.Process.GetProcessesByName(name);
            if (procs.Length > 0)
                return new LocalProcess(procs[0]);
            return null;
        }

        public static async Task<LocalProcess> WaitForProcess(string name)
        {
            while (true)
            {
                var proc = Find(name);
                if (proc != null) return proc;

                await Task.Delay(TimeSpan.FromMilliseconds(500));
            }
        }

        private WinAPI.MEMORY_BASIC_INFORMATION[] GetPages()
        {
            var handle = WinAPI.OpenProcess(WinAPI.ProcessAccessFlags.QueryInformation, false, PID);
            if (handle.ToInt64() <= 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            var pages = WinAPI.EnumeratePages(handle).ToArray();
            WinAPI.CloseHandle(handle);
            return pages;
        }

        public IMemory CreateMemoryInterface(WinAPI.ProcessAccessFlags flags, bool raiseExceptions)
        {
            return new LocalMemory(this, raiseExceptions, flags);
        }
        public IMemory CreateMemoryInterface()
        {
            return CreateMemoryInterface(WinAPI.ProcessAccessFlags.VirtualMemoryRead | WinAPI.ProcessAccessFlags.VirtualMemoryWrite, true);
        }

        public void UpdatePages()
        {
            pages = GetPages().ToArray();
        }

        public Task<bool> Execute(Pointer functionAddress, Pointer parameterAddress)
        {
            return Task.Run(() =>
            {
                var thread = WinAPI.CreateRemoteThread(execHandle, IntPtr.Zero, IntPtr.Zero, functionAddress, parameterAddress, 0, IntPtr.Zero);
                bool success = false;
                if (thread != IntPtr.Zero)
                {
                    bool failed = WinAPI.WaitForSingleObject(thread, 0xFFFFFFFF) == 0xFFFFFFFF;
                    success = !failed;
                }
                WinAPI.CloseHandle(thread);
                return success;
            });
        }

        public override string ToString()
        {
            return $"[{PID}] {Name}";
        }

        public IModule FindModule(string name)
        {
            return Modules.FirstOrDefault(x => x.Name == name);
        }
    }
}
