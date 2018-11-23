using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Controls.Events
{
    public class ControlEventArgs : EventArgs
    {
        public Control Control { get; private set; }

        public ControlEventArgs(Control control)
        {
            Control = control;
        }
    }
}
