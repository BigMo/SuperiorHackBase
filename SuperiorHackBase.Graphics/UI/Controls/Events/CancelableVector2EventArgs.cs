using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperiorHackBase.Core.Maths;

namespace SuperiorHackBase.Graphics.UI.Controls.Events
{
    public class CancelableVector2EventArgs : Vector2EventArgs, ICancelable
    {
        public CancelableVector2EventArgs(Vector2 oldValue, Vector2 newValue) : base(oldValue, newValue)
        {
        }

        public bool Cancel { get; set; }
    }
}
