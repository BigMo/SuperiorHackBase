using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Controls.Events
{
    public class CancelableValueEventArgs<T> : ValueEventArgs<T>, ICancelable
    {
        public bool Cancel { get; set; }

        public CancelableValueEventArgs(T oldValue, T newValue) : base(oldValue, newValue) { }
    }
}
