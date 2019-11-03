using SuperiorHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperiorHackBase.Input
{
    public class MouseEventExtArgs : System.Windows.Forms.MouseEventArgs
    {
        public MouseEventExtArgs()
            : base(MouseButtons.None, 0, 0, 0, 0)
        {
        }

        public MouseEventExtArgs(MouseButtons b, int clickcount, WinAPI.POINT point, int delta)
            : base(b, clickcount, point.X, point.Y, delta)
        {
        }
        public MouseEventExtArgs(MouseButtons b, int clickcount, int x, int y, int delta)
            : base(b, clickcount, x, y, delta)
        {
        }

        /// <summary>
        /// If mouse wheel moved
        /// </summary>
        public bool Wheel;

        /// <summary>
        /// Used to check if button is released or pressed
        /// If Wheel equals true then shows which way wheel is being turned
        /// </summary>
        public UpDown UpOrDown = UpDown.None;
        public enum UpDown
        {
            None,
            Up,
            Down
        }
    }
}
