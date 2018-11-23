using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Controls.Events
{
    public class CancelableControlEventArgs : ControlEventArgs, ICancelable
    {
        public bool Cancel { get; set; }

        public CancelableControlEventArgs(Control control) : base(control) { }
    }
}
