using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Controls.Events
{
    public class CancelableStringEventArgs : StringEventArgs, ICancelable
    {
        public bool Cancel { get; set; }

        public CancelableStringEventArgs(string oldValue, string newValue) : base(oldValue, newValue)
        {
        }
    }
}
