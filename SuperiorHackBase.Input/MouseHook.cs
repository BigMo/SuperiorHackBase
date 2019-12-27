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
    public class MouseHook : WindowsHook
    {
        public event EventHandler<MouseEventExtArgs> MouseEvent;

        public MouseHook() : base(WinAPI.HookType.WH_MOUSE_LL)
        {
        }

        protected override IntPtr OnHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                //Marshall the data from callback.
                WinAPI.MouseLLHookStruct mouseHookStruct = (WinAPI.MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(WinAPI.MouseLLHookStruct));

                MouseButtons button = MouseButtons.None;
                short mouseDelta = 0;
                int clickCount = 0;
                UpDown upDown = UpDown.None;

                //detect button clicked
                switch ((WinAPI.WindowMessage)wParam)
                {
                    case WinAPI.WindowMessage.WM_LBUTTONDOWN:
                        upDown = UpDown.Down;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WinAPI.WindowMessage.WM_LBUTTONUP:
                        upDown = UpDown.Up;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WinAPI.WindowMessage.WM_LBUTTONDBLCLK:
                        button = MouseButtons.Left;
                        clickCount = 2;
                        break;
                    case WinAPI.WindowMessage.WM_RBUTTONDOWN:
                        upDown = UpDown.Down;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WinAPI.WindowMessage.WM_RBUTTONUP:
                        upDown = UpDown.Up;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WinAPI.WindowMessage.WM_RBUTTONDBLCLK:
                        button = MouseButtons.Right;
                        clickCount = 2;
                        break;
                    case WinAPI.WindowMessage.WM_MOUSEWHEEL:
                        //If the message is WM_MOUSEWHEEL, the high-order word of MouseData member is the wheel delta. 
                        //One wheel click is defined as WHEEL_DELTA, which is 120. 
                        //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
                        if (mouseDelta > 0)
                            upDown = UpDown.Up;
                        if (mouseDelta < 0)
                            upDown = UpDown.Down;
                        //TODO: X BUTTONS (I havent them so was unable to test)
                        //If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, 
                        //or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
                        //and the low-order word is reserved. This value can be one or more of the following values. 
                        //Otherwise, MouseData is not used. 
                        break;
                }
                var args = new MouseEventExtArgs(
                                                   button,
                                                   clickCount,
                                                   mouseHookStruct.Point.X,
                                                   mouseHookStruct.Point.Y,
                                                   mouseDelta,
                                                   upDown);
                
                MouseEvent.Invoke(this, args);
            }
            //call next hook
            return WinAPI.CallNextHookEx(hHook, nCode, wParam, lParam);
        }
    }
}
