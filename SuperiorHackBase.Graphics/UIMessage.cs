using SuperiorHackBase.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperiorHackBase.Graphics
{
    public class UIMessage
    {
        public KeyEventExtArgs KeyEvent { get; private set; }
        public MouseEventExtArgs MouseEvent { get; private set; }
        public bool HasKeyEvent => KeyEvent != null;
        public bool HasMouseEvent => MouseEvent != null;

        public UIMessage(KeyEventExtArgs keyEvent, MouseEventExtArgs mouseEvent)
        {
            KeyEvent = keyEvent;
            MouseEvent = mouseEvent;
        }
        public UIMessage(KeyEventExtArgs keyEvent) : this(keyEvent, null) { }
        public UIMessage(MouseEventExtArgs mouseEvent) : this(null, mouseEvent) { }
    }
}
