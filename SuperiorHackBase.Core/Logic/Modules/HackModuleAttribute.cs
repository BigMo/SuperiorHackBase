using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.Logic.Modules
{
    public class HackModuleAttribute : Attribute
    {
        public int TicksPerSecond { get; private set; }

        public HackModuleAttribute(int ticksPerSecond)
        {
            TicksPerSecond = ticksPerSecond;
        }
    }
}
