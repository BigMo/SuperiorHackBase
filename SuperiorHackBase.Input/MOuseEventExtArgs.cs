using SuperiorHackBase.Core;
using SuperiorHackBase.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperiorHackBase.Input
{
    public class MouseEventExtArgs : EventArgs
    {
        public Vector2 Position { get; private set; }
        public MouseButtons Button { get; private set; }
        public int Clicks { get; private set; }
        public int WheelDelta { get; private set; }
        public bool WheelMoved => WheelDelta != 0;
        public UpDown UpOrDown { get; private set; } = UpDown.None;
        
        public MouseEventExtArgs MakeRelative(Vector2 to)
        {
            var newPos = Position - to;
            return new MouseEventExtArgs(Button, Clicks, (int)newPos.X, (int)newPos.Y, WheelDelta, UpOrDown);
        }
        public MouseEventExtArgs(MouseButtons b, int clickcount, int x, int y, int delta, UpDown upDown)
        {
            Button = b;
            Clicks = clickcount;
            Position = new Vector2(x, y);
            WheelDelta = delta;
            UpOrDown = upDown;
        }
    }
}
