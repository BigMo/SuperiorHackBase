using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperiorHackBase.Core.Maths;

namespace SuperiorHackBase.Graphics.Controls.Layouts
{
    public abstract class Layout : Control
    {
        public Distance Padding
        {
            get => padding;
            set
            {
                if (padding != value)
                {
                    padding = value;
                    RearrangeControls();
                }
            }
        }
        public Distance ChildMargins
        {
            get => childMargins;
            set
            {
                if (childMargins != value)
                {
                    childMargins = value;
                    RearrangeControls();
                }
            }
        }
        private Distance padding;
        private Distance childMargins;
        
        public Layout()
        {
            padding = new Distance(4f);
            childMargins = new Distance(4f);
            ParentChanged += Layout_ParentChanged;
        }

        private void Layout_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null) Parent.SizeChanged += Parent_SizeChanged;
        }

        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            Size = Parent.Size;
        }

        public abstract void RearrangeControls();

        public override Control AddChild(Control child)
        {
            child = base.AddChild(child);
            if (child != null)
            {
                RearrangeControls();
                child.SizeChanged += Child_SizeChanged;
            }
            return child;
        }

        public override Control RemoveChild(Control child)
        {
            child = base.RemoveChild(child);
            if (child != null)
            {
                RearrangeControls();
                child.SizeChanged -= Child_SizeChanged;
            }
            return child;
        }

        private void Child_SizeChanged(object sender, EventArgs e)
        {
            RearrangeControls();
        }

        protected override void AfterResize(Vector2 oldSize, Vector2 newSize)
        {
            base.AfterResize(oldSize, newSize);
            RearrangeControls();
        }


    }
}
