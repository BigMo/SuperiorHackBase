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
        //Protected events to intercept and, if so desired, cancel changes
        protected event EventHandler<CancelableValueEventArgs<Vector2>> PositionChanging;
        protected event EventHandler<CancelableValueEventArgs<Vector2>> SizeChanging;
        protected event EventHandler<CancelableValueEventArgs<string>> TextChanging;
        protected event EventHandler<CancelableValueEventArgs<Dock>> DockChanging;
        protected event EventHandler<CancelableValueEventArgs<bool>> AutoSizeChanging;
        protected event EventHandler<CancelableValueEventArgs<Vector2>> MarginChanging;
        protected event EventHandler<CancelableValueEventArgs<Vector2>> PaddingChanging;
        protected event EventHandler<CancelableControlEventArgs> ChildAdding;
        protected event EventHandler<CancelableControlEventArgs> ChildRemoving;

        //Public events for use by any other class/component
        public event EventHandler<ValueEventArgs<Vector2>> PositionChanged;
        public event EventHandler<ValueEventArgs<Vector2>> SizeChanged;
        public event EventHandler<ValueEventArgs<string>> TextChanged;
        public event EventHandler<ValueEventArgs<Dock>> DockChanged;
        public event EventHandler<ValueEventArgs<bool>> AutoSizeChanged;
        public event EventHandler<ValueEventArgs<Vector2>> MarginChanged;
        public event EventHandler<ValueEventArgs<Vector2>> PaddingChanged;
        protected event EventHandler<ControlEventArgs> ChildAdded;
        protected event EventHandler<ControlEventArgs> ChildRemoved;
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

        public Dock Dock
        {
            get { return dock; }
            set
            {
                if(value != dock)
                    OnDockChange(dock, value);
            }
        }
        public bool AutoSize
        {
            get { return autoSize; }
            set
            {
                if (autoSize != value)
                    OnAutoSizeChange(autoSize, value);
            }
        }

        public Vector2 Margin
        {
            get { return margin; }
            set
            {
                if (value != margin)
                    OnMarginChange(margin, value);
            }
        }
        public Vector2 Padding
        {
            get { return padding; }
            set
            {
                if (value != padding)
                    OnPaddingChange(padding, value);
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

        public bool IsDocked { get { return dock != Dock.None; } }

        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                    OnTextChange(text, value);
            }
        }
        public IFont Font { get; set; }
        #endregion

        #region VARIABLES
        private float x;
        private float y;
        private float width;
        private float height;
        private List<Control> children;
        private Control parent;
        private string text;
        private bool enabled;
        private Dock dock;
        private bool autoSize;
        private Vector2 margin;
        private Vector2 padding;
        //TODO: Font, BackColor, ForeColor like .NET?
        //TODO: Docking? Alignment? Layouts?
        #endregion

        #region CONSTRUCTORS
        public Control()
        {
            x = y = width = height = 0f;
            children = new List<Control>();
            parent = null;
            text = this.GetType().Name;
            enabled = true;
            dock = Dock.None;
            autoSize = false;
        }
        #endregion

        #region ABSTRACT METHODS
        public abstract void Draw(IControlPaint controlPaint); //TODO: Interface?
        protected abstract Vector2 CalculateSize();
        #endregion

        #region PUBLIC METHODS
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

            OnChildAdd(child);
        }
        public void RemoveChild(Control child)
        {
            if (child == null || !children.Contains(child))
                return;

            OnChildRemove(child);
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

        #region PRIVATE METHODS
        private void AdjustDocking()
        {
            if (parent == null)
                return;

            switch (dock)
            {
                case Dock.Top:
                    Reposition(0, 0);
                    Resize(parent.Width, height);
                    break;
                case Dock.Bottom:
                    Reposition(0, parent.Height - height);
                    Resize(parent.Width, height);
                    break;
                case Dock.Left:
                    Reposition(0, 0);
                    Resize(width, parent.Height);
                    break;
                case Dock.Right:
                    Reposition(parent.Width - width, 0);
                    Resize(width, Parent.Height);
                    break;
                case Dock.Fill:
                    Reposition(0, 0);
                    Resize(parent.Width, parent.Height);
                    break;
            }
        }
        private void AdjustAutoSize()
        {
            if (autoSize)
            {
                var size = CalculateSize();
                Resize(size);
            }
        }

        private void Resize(Vector2 size)
        {
            Resize(size.X, size.Y);
        }
        private void Resize(float newWidth, float newHeight)
        {
            //Sanity checks, e.g. clamping
            if (newWidth != width || newHeight != height)
            {
                var oldWidth = width;
                var oldHeight = height;
                width = newWidth;
                height = newHeight;
                OnSizeChanged(oldWidth, oldHeight, width, height);
            }
        }

        private void Reposition(Vector2 position)
        {
            Reposition(position.X, position.Y);
        }
        private void Reposition(float newX, float newY)
        {
            //Sanity checks, e.g. clamping
            if (newX != x || newY != y)
            {
                var oldX = x;
                var oldY = y;
                x = newX;
                y = newY;
                OnPositionChanged(oldX, oldY, x, y);
            }
        }

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
        #endregion

        #region VIRTUAL METHODS
        protected virtual void OnPositionChange(float oldX, float oldY, float newX, float newY, bool allowCancel = true)
        {
            if (IsDocked)
                return;

            if (allowCancel && PositionChanging != null)
            {
                var args = new CancelableValueEventArgs<Vector2>(new Vector2(oldX, oldY), new Vector2(newX, newY));
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
            PositionChanged?.Invoke(this, new ValueEventArgs<Vector2>(new Vector2(oldX, oldY), new Vector2(newX, newY)));

            foreach (var child in children)
                child.OnParentPositionChanged(oldX, oldY, newX, newY);
        }
        protected virtual void OnSizeChange(float oldWidth, float oldHeight, float newWidth, float newHeight, bool allowCancel = true)
        {
            if (autoSize || IsDocked)
                return;

            if (allowCancel && SizeChanging != null)
            {
                var args = new CancelableValueEventArgs<Vector2>(new Vector2(oldWidth, oldHeight), new Vector2(newWidth, newHeight));
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
            SizeChanged?.Invoke(this, new ValueEventArgs<Vector2>(new Vector2(oldWidth, oldHeight), new Vector2(newWidth, newHeight)));

            foreach (var child in children)
                child.OnParentSizeChanged(oldWidth, oldHeight, newWidth, newHeight);
        }
        protected virtual void OnTextChange(string oldText, string newText, bool allowCancel = true)
        {
            if (allowCancel && TextChanging != null)
            {
                var args = new CancelableValueEventArgs<string>(oldText, newText);
                TextChanging(this, args);
                if (args.Cancel)
                    return;
            }
            this.text = newText;

            OnTextChanged(oldText, newText);
        }
        protected virtual void OnTextChanged(string oldText, string newText)
        {
            TextChanged?.Invoke(this, new ValueEventArgs<string>(oldText, newText));
        }
        protected virtual void OnDockChange(Dock oldDock, Dock newDock, bool allowCancel = true)
        {
            if (allowCancel && DockChanging != null)
            {
                var args = new CancelableValueEventArgs<Dock>(oldDock, newDock);
                DockChanging(this, args);
                if (args.Cancel)
                    return;
            }
            this.dock = newDock;

            OnDockChanged(oldDock, newDock);
        }
        protected virtual void OnDockChanged(Dock oldDock, Dock newDock)
        {
            AdjustDocking();
            DockChanged?.Invoke(this, new ValueEventArgs<Dock>(oldDock, newDock));
        }
        protected virtual void OnAutoSizeChange(bool oldAutoSize, bool newAutoSize, bool allowCancel = true)
        {
            if (allowCancel && AutoSizeChanging != null)
            {
                var args = new CancelableValueEventArgs<bool>(oldAutoSize, newAutoSize);
                AutoSizeChanging(this, args);
                if (args.Cancel)
                    return;
            }
            this.autoSize = newAutoSize;

            OnAutoSizeChanged(oldAutoSize, newAutoSize);
        }
        protected virtual void OnAutoSizeChanged(bool oldAutoSize, bool newAutoSize)
        {
            AdjustAutoSize();
            AutoSizeChanged?.Invoke(this, new ValueEventArgs<bool>(oldAutoSize, newAutoSize));
        }
        protected virtual void OnMarginChange(Vector2 oldMargin, Vector2 newMargin, bool allowCancel = true)
        {
            if (allowCancel && MarginChanging != null)
            {
                var args = new CancelableValueEventArgs<Vector2>(oldMargin, newMargin);
                MarginChanging(this, args);
                if (args.Cancel)
                    return;
            }
            this.margin = newMargin;

            OnMarginChanged(oldMargin, newMargin);
        }
        protected virtual void OnMarginChanged(Vector2 oldMargin, Vector2 newMargin)
        {
            MarginChanged?.Invoke(this, new ValueEventArgs<Vector2>(oldMargin, newMargin));
        }
        protected virtual void OnPaddingChange(Vector2 oldPadding, Vector2 newPadding, bool allowCancel = true)
        {
            if (allowCancel && PaddingChanging != null)
            {
                var args = new CancelableValueEventArgs<Vector2>(oldPadding, newPadding);
                PaddingChanging(this, args);
                if (args.Cancel)
                    return;
            }
            this.padding = newPadding;

            OnPaddingChanged(oldPadding, newPadding);
        }
        protected virtual void OnPaddingChanged(Vector2 oldPadding, Vector2 newPadding)
        {
            PaddingChanged?.Invoke(this, new ValueEventArgs<Vector2>(oldPadding, newPadding));
        }
        protected virtual void OnChildAdd(Control child, bool allowCancel = true)
        {
            if (allowCancel && ChildAdding != null)
            {
                var args = new CancelableControlEventArgs(child);
                ChildAdding(this, args);
                if (args.Cancel)
                    return;
            }

            children.Add(child);
            child.Parent = this;

            OnChildAdded(child);
        }
        protected virtual void OnChildAdded(Control child)
        {
            ChildAdded?.Invoke(this, new ControlEventArgs(child));
        }
        protected virtual void OnChildRemove(Control child, bool allowCancel = true)
        {
            if (allowCancel && ChildAdding != null)
            {
                var args = new CancelableControlEventArgs(child);
                ChildAdding(this, args);
                if (args.Cancel)
                    return;
            }

            children.Remove(this);
            child.parent = null;

            OnChildRemoved(child);
        }
        protected virtual void OnChildRemoved(Control child)
        {
            ChildRemoved?.Invoke(this, new ControlEventArgs(child));
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