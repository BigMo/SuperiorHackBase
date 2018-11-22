using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Controls.Events
{
    public abstract class ValueChangeEventArgs<T>
    {
        public T OldValue { get; private set; }
        public T NewValue { get; private set; }

        public ValueChangeEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
