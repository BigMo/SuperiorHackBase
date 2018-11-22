using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Controls.Events
{
    public class StringEventArgs : ValueChangeEventArgs<string>
    {
        public int LengthDelta
        {
            get
            {
                if (!string.IsNullOrEmpty(OldValue) && !string.IsNullOrEmpty(NewValue))
                {
                    return NewValue.Length - OldValue.Length;
                } else if (string.IsNullOrEmpty(OldValue) && !string.IsNullOrEmpty(NewValue))
                {
                    return NewValue.Length;
                } else if (!string.IsNullOrEmpty(OldValue) && string.IsNullOrEmpty(NewValue))
                {
                    return -OldValue.Length;
                }
                else
                {
                    return 0;
                }
            }
        }
        public StringEventArgs(string oldValue, string newValue) : base(oldValue, newValue)
        {
        }
    }
}
