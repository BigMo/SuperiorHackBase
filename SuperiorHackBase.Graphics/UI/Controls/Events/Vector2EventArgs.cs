using SuperiorHackBase.Core.Maths;
using System;

namespace SuperiorHackBase.Graphics.UI.Controls.Events
{
    public class Vector2EventArgs : ValueChangeEventArgs<Vector2>
    {
        public Vector2 Delta { get { return NewValue - OldValue; } }

        public Vector2EventArgs(Vector2 oldValue, Vector2 newValue) : base(oldValue, newValue) { }
    }
}