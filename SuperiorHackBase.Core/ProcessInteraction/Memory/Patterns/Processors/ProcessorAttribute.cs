using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns.Processors
{
    public class ProcessorAttribute : Attribute
    {
        public int Pops { get; set; }
        public int Pushes { get; set; }

        public ProcessorAttribute()
        {
            Pops = 0;
            Pushes = 0;
        }
    }
}
