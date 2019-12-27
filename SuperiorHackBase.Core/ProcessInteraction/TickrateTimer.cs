using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperiorHackBase.Core.ProcessInteraction
{
    public class TickrateTimer : Timer
    {
        public float Tickrate { get => 1000f / Interval; set => Interval = (int)(1000f / value); }

        public TickrateTimer()
        {
            Tickrate = 100f;
        }
    }
}
