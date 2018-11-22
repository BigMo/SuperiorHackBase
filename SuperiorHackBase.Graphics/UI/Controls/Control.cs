using SuperiorHackBase.Core.Maths;
using SuperiorHackBase.Graphics.UI.Controls.Events;
using SuperiorHackBase.Graphics.UI.Painting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.UI.Controls
{
    public abstract class Control
    {
        #region EVENTS
        protected event EventHandler<CancelableVector2EventArgs> PositionChanging;
        public event EventHandler<Vector2EventArgs> PositionChanged;
        protected event EventHandler<CancelableVector2EventArgs> SizeChanging;
        public event EventHandler<Vector2EventArgs> SizeChanged;
        protected event EventHandler<CancelableStringEventArgs> TextChanging;
        public event EventHandler<StringEventArgs> TextChanged;
        public event EventHandler EnabledChanged;
        #endregion

        #region PROPERTIES
        public bool Enabled
        {
            get { return parent != null ? parent.Enabled && enabled : enabled; }
            set
            {
                if (enabled != value)
                    OnEnabledChanged(value);
            }
        }
        public float X
        {
            get { return x; }
            set
            {
                if (x != value)
                    OnPositionChange(x, y, value, y);
            }
        }
        public float Y
        {
            get { return y; }
            set
            {
                if (y != value)
                    OnPositionChange(x, y, x, value);
            }
        }
        public float Width
        {
            get { return width; }
            set
            {
                if (width != value)
                    OnSizeChange(width, height, value, height);
            }
        }
        public float Height
        {
            get { return height; }
            set
            {
                if (height != value)
                    OnSizeChange(width, height, width, value);
            }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle(x, y, width, height); }
            set
            {
                var bounds = Bounds;
                if (bounds != value)
                {
                    if (x != value.X || y != value.Y)
                        OnPositionChange(x, y, value.X, value.Y);

                    if (width != value.Width || height != value.Height)
                        OnSizeChange(width, height, value.Width, value.Height);
                }
            }
        }

        public Vector2 AbsolutePosition
        {
            get { return parent != null ? parent.AbsolutePosition + Position : Position; }
        }
        public Vector2 AbsoluteOrigin
        {
            get { return parent != null ? parent.AbsolutePosition : Vector2.Zero; }
        }

        public Vector2 Position
        {
            get { return new Vector2(x, y); }
            set
            {
                if (x != value.X || y != value.Y)
                    OnPositionChange(x, y, value.X, value.Y);
            }
        }
        public Vector2 Size
        {
            get { return new Vector2(width, height); }
            set
            {
                if (width != value.X || height != value.Y)
                    OnSizeChange(width, height, value.X, value.Y);
            }
        }

        public IEnumerable<Control> Children { get { return children; } }
        public Control Parent
        {
            get { return parent; }
            set
            {
                if (value != parent)
                    SetParent(value);
            }
        }
        public Control Root
        {
            get { return Parent != null ? Parent.Root : this; }
        }
        #endregion

        #region VARIABLES
        private float x, y, width, height;
        private List<Control> children;
        private Control parent;
        private string text;
        private bool enabled;
        //TODO: Font, BackColor, ForeColor like .NET?
        //TODO: Docking? Alignment? Layouts?
        #endregion

        #region CONSTRUCTORS
        public Control()
        {
            x = y = width = height = 0f;
            parent = null;
            children = new List<Control>();
            text = this.GetType().Name;
            enabled = true;
        }
        #endregion

        #region METHODS
        public abstract void Draw(IControlPaint controlPaint); //TODO: Interface?

        private void SetParent(Control parent)
        {
            if (parent == null)
            {
                if (this.parent != null)
                    this.parent.RemoveChild(this);
                this.parent = parent;
            }
            else
            {
                if (parent == this)
                    throw new ArgumentException("Can not be its own parent");
                if (ContainsInChildren(parent))
                    throw new ArgumentException("Can not be child of own child");
                this.parent = parent;
                this.parent.AddChild(this);
            }
        }

        public bool Contains(Control control)
        {
            return children.Contains(control);
        }
        public bool ContainsInChildren(Control control)
        {
            return Contains(control) || children.Any(x => x.ContainsInChildren(control));
        }
        public void AddChild(Control child)
        {
            if (child == null || children.Contains(child))
                return;

            children.Add(child);
            child.Parent = this;
        }
        public void RemoveChild(Control child)
        {
            if (child == null || !children.Contains(child))
                return;

            children.Remove(this);
            child.parent = null;
        }

        public Vector2 ClientToScreen(Vector2 point)
        {
            return point + AbsolutePosition;
        }
        public Vector2 ScreenToClient(Vector2 point)
        {
            return point - AbsolutePosition;
        }
        #endregion

        #region VIRTUAL METHODS
        protected virtual void OnPositionChange(float oldX, float oldY, float newX, float newY)
        {
            if (PositionChanging != null)
            {
                var args = new CancelableVector2EventArgs(new Vector2(oldX, oldY), new Vector2(newX, newY));
                PositionChanging(this, args);
                if (args.Cancel)
                    return;
            }
            this.x = newX;
            this.y = newY;

            OnPositionChanged(oldX, oldY, newX, newY);
        }
        protected virtual void OnPositionChanged(float oldX, float oldY, float newX, float newY)
        {
            PositionChanged?.Invoke(this, new Vector2EventArgs(new Vector2(oldX, oldY), new Vector2(newX, newY)));

            foreach (var child in children)
                child.OnParentPositionChanged(oldX, oldY, newX, newY);
        }
        protected virtual void OnSizeChange(float oldWidth, float oldHeight, float newWidth, float newHeight)
        {
            if (SizeChanging != null)
            {
                var args = new CancelableVector2EventArgs(new Vector2(oldWidth, oldHeight), new Vector2(newWidth, newHeight));
                SizeChanging(this, args);
                if (args.Cancel)
                    return;
            }
            this.width = newWidth;
            this.height = newHeight;

            OnSizeChanged(oldWidth, oldHeight, newWidth, newHeight);
        }
        protected virtual void OnSizeChanged(float oldWidth, float oldHeight, float newWidth, float newHeight)
        {
            SizeChanged?.Invoke(this, new Vector2EventArgs(new Vector2(oldWidth, oldHeight), new Vector2(newWidth, newHeight)));

            foreach (var child in children)
                child.OnParentSizeChanged(oldWidth, oldHeight, newWidth, newHeight);
        }
        protected virtual void OnTextChange(string oldText, string newText)
        {
            if (TextChanging != null)
            {
                var args = new CancelableStringEventArgs(oldText, newText);
                TextChanging(this, args);
                if (args.Cancel)
                    return;
            }
            this.text = newText;

            OnTextChanged(oldText, newText);
        }
        protected virtual void OnTextChanged(string oldText, string newText)
        {
            TextChanged?.Invoke(this, new StringEventArgs(oldText, newText));
        }
        protected virtual void OnEnabledChanged(bool enabled)
        {
            this.enabled = enabled;
            EnabledChanged?.Invoke(this, EventArgs.Empty);

            foreach (var child in children)
                child.OnParentEnabledChanged(enabled);
        }

        protected virtual void OnParentEnabledChanged(bool enabled) { }
        protected virtual void OnParentPositionChanged(float oldX, float oldY, float newX, float newY) { }
        protected virtual void OnParentSizeChanged(float oldWidth, float oldHeight, float newWidth, float newHeight) { }
        #endregion
    }
}