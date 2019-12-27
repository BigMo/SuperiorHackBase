using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Graphics
{
    public class RenderingEventArgs : EventArgs
    {
        public Renderer Renderer { get; private set; }
        public GameOverlay Overlay { get; private set; }
        public TimeSpan Delta { get; private set; }
        public RenderingEventArgs(Renderer renderer, GameOverlay overlay, TimeSpan delta)
        {
            Renderer = renderer;
            Overlay = overlay;
            Delta = delta;
        }
    }
}
