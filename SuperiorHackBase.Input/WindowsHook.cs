using SuperiorHackBase.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static SuperiorHackBase.Core.WinAPI;

namespace SuperiorHackBase.Input
{
    public abstract class WindowsHook : IDisposable
    {
        public HookType Type { get; private set; }
        protected IntPtr hHook { get; private set; }
        private HookProc hProc;

        protected WindowsHook(HookType type)
        {
            Type = type;
        }

        public void Hook()
        {
            var user32 = WinAPI.GetModuleHandle("user32");
            hProc = new WinAPI.HookProc(HookCallback);
            hHook = WinAPI.SetWindowsHookEx(Type, hProc, user32, 0);

            if (hHook == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        public void Unhook()
        {
            WinAPI.UnhookWindowsHookEx(hHook);
        }

        private IntPtr HookCallback(int nCode, int wParam, IntPtr lParam)
        {
            return OnHook(nCode, wParam, lParam);
        }

        protected abstract IntPtr OnHook(int nCode, int wParam, IntPtr lParam);

        public void Dispose()
        {
            Unhook();
        }
    }
}
