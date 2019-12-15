using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperiorHackBase.Input
{
    public class KeyEventExtArgs : EventArgs
    {
        public Keys Key { get; private set; }
        public UpDown UpOrDown { get; private set; } = UpDown.None;

        public KeyEventExtArgs(Keys key, UpDown upDown)
        {
            Key = key;
            UpOrDown = upDown;
        }

        public override string ToString()
        {
            return $"Key: {Key}, UpOrDown: {UpOrDown}";
        }
    }
}
