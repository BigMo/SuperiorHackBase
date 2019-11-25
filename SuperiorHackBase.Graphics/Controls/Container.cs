using SuperiorHackBase.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics.Controls
{
    public abstract class Container
    {
        public IEnumerable<Control> Children => children;
        private List<Control> children;

        public Container()
        {
            children = new List<Control>();
        }

        public virtual Control RemoveChild(Control child)
        {
            if (child == null || !children.Contains(child))
                return null;

            child.Parent = null;
            children.Remove(child);
            return child;
        }
        public virtual Control AddChild(Control child)
        {
            if (child == null || child == this || children.Contains(child)) return null;
            children.Add(child);
            return child;
        }
        public virtual void Draw(Renderer renderer)
        {
            DrawChildren(renderer);
        }
        protected virtual void DrawChildren(Renderer renderer)
        {
            foreach (var c in Children.Where(x => x.Enabled))
                c.Draw(renderer);
        }
    }
}
