using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SuperiorHackBase.Core.Maths;
using SuperiorHackBase.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Controls
{
    public class Control : Container
    {
        #region EVENTS
        public event EventHandler TextChanged;
        public event EventHandler LocationChanged;
        public event EventHandler SizeChanged;
        public event EventHandler BackColorChanged;
        public event EventHandler ForeColorChanged;
        public event EventHandler EnabledChanged;
        public event EventHandler VisibleChanged;
        public event EventHandler TagChanged;
        public event EventHandler FontChanged;
        public event EventHandler ParentChanged;
        public event EventHandler<MouseEventExtArgs> MouseEntered;
        public event EventHandler<MouseEventExtArgs> MouseLeft;
        public event EventHandler<MouseEventExtArgs> MouseDown;
        public event EventHandler<MouseEventExtArgs> MouseUp;
        #endregion //EVENTS

        #region PROPERTIES
        public string Text
        {
            get => text;
            set
            {
                if (value != text)
                {
                    text = value;
                    TextChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public Vector2 Location
        {
            get => location;
            set
            {
                if (value != location)
                {
                    location = value;
                    LocationChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public Vector2 Size
        {
            get => size;
            set
            {
                if (value != size)
                {
                    var old = size;
                    if (!BeforeResize(old, value)) return;
                    size = value;
                    AfterResize(old, value);
                    SizeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public Rectangle Rectangle
        {
            get => new Rectangle(Location, Size);
            set 
            {
                Location = value.Position;
                Size = value.Size;
            }
        }
        public BrushDescription BackColor
        {
            get => backColor;
            set
            {
                if (!value.Equals(backColor))
                {
                    backColor = value;
                    BackColorChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public BrushDescription ForeColor
        {
            get => foreColor;
            set
            {
                if (!value.Equals(backColor))
                {
                    foreColor = value;
                    ForeColorChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (value != enabled)
                {
                    enabled = value;
                    EnabledChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public bool Visible
        {
            get => visible;
            set
            {
                if (value != visible)
                {
                    visible = value;
                    VisibleChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public object Tag
        {
            get => tag;
            set
            {
                if (value != tag)
                {
                    tag = value;
                    TagChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public FontDescription Font
        {
            get => font;
            set
            {
                if (value != font)
                {
                    font = value;
                    FontChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public Control Parent
        {
            get => parent;
            set
            {
                if (value != parent)
                {
                    var oldParent = parent;
                    parent = value;
                    if (oldParent != null) oldParent.RemoveChild(value);
                    if (parent != null) parent.AddChild(value);
                    ParentChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public Control Root => parent?.Root ?? this;
        public Vector2 AbsoluteLocation => parent != null ? Location + parent.AbsoluteLocation : Location;
        public float Width => Size.X;
        public float Height => Size.Y;
        public float X => Location.X;
        public float Y => Location.Y;
        public bool MouseOver
        {
            get => mouseOver;
        }
        #endregion //PROPERTIES

        #region FIELDS
        private string text = "";
        private Vector2 location = Vector2.Zero;
        private Vector2 size = Vector2.Unit;
        private BrushDescription backColor = BrushDescription.White;
        private BrushDescription foreColor = BrushDescription.Black;
        private bool enabled = true;
        private bool visible = true;
        private object tag = null;
        private FontDescription font = null;
        private Control parent = null;
        private bool mouseOver;
        private bool mouseDown;
        #endregion //FIELDS

        public Control()
        {
            enabled = true;
            visible = true;
            font = FontDescription.DEBUG;
            backColor = BrushDescription.White;
            foreColor = BrushDescription.Black;

#if DEBUG
            MouseEntered += (o, e) => Console.WriteLine("[{0}] MouseEntered", GetType().Name);
            MouseLeft += (o, e) => Console.WriteLine("[{0}] MouseLeft", GetType().Name);
            MouseDown += (o, e) => Console.WriteLine("[{0}] MouseDown", GetType().Name);
            MouseUp += (o, e) => Console.WriteLine("[{0}] MouseUp", GetType().Name);
#endif
        }

        public override void Draw(Renderer renderer)
        {
            if (!visible) return;
            DrawBackground(renderer);
            DrawForeground(renderer);
            DrawChildren(renderer);
        }

        protected virtual void DrawBackground(Renderer renderer)
        {
        }

        protected virtual void DrawForeground(Renderer renderer)
        {
        }

        protected virtual bool BeforeResize(Vector2 oldSize, Vector2 newSize)
        {
            return true;
        }

        protected virtual void AfterResize(Vector2 oldSize, Vector2 newSize) { }

        public override Control AddChild(Control child)
        {
            if (base.AddChild(child) == null) return null;
            child.Parent = this;
            return child;
        }
        protected bool IsMouseOver(Vector2 cursor)
        {
            return Rectangle.Intersects(cursor);
        }

        private void SetMouseDown(bool isDown, MouseEventExtArgs e)
        {
            if (mouseDown != isDown)
            {
                mouseDown = isDown;
                if (isDown) MouseDown?.Invoke(this, e);
                else MouseUp?.Invoke(this, e);
            }
        }

        public void SetMouseOver(bool isOver, MouseEventExtArgs e)
        {
            if (mouseOver != isOver)
            {
                mouseOver = isOver;
                if (!mouseOver)
                    foreach (var c in Children) c.SetMouseOver(false, e);
                if (mouseOver && Parent != null) Parent.SetMouseOver(true, e);

                //Events
                if (mouseOver) MouseEntered?.Invoke(this, e);
                else MouseLeft?.Invoke(this, e);
            }
        }

        public Control GetMouseControl(Vector2 pos)
        {
            if (!Rectangle.Intersects(pos))
                return null;

            Control control = null;
            if (Children.Any(x => (control = x.GetMouseControl(pos)) != null))
                return control;
            
            return this;
        }
    }
}
