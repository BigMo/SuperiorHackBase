using SuperiorHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperiorHackBase.Input
{
    public class KeyboardHook : WindowsHook
    {
        public event EventHandler<KeyEventExtArgs> KeyUp;
        public event EventHandler<KeyEventExtArgs> KeyDown;

        public KeyboardHook() : base(WinAPI.HookType.WH_KEYBOARD_LL)
        {
        }

        protected override IntPtr OnHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                switch ((WinAPI.WindowMessage)wParam)
                {
                    case WinAPI.WindowMessage.WM_KEYDOWN:
                        KeyDown?.Invoke(this, new KeyEventExtArgs((Keys)vkCode, UpDown.Down));
                        break;
                    case WinAPI.WindowMessage.WM_KEYUP:
                        KeyUp?.Invoke(this, new KeyEventExtArgs((Keys)vkCode, UpDown.Up));
                        break;
                }
            }

            return WinAPI.CallNextHookEx(hHook, nCode, wParam, lParam);
        }
    }
}
