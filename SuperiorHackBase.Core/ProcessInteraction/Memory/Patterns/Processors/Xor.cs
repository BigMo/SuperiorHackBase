using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns.Processors
{
    public class Xor : ArithmeticProcessor
    {
        protected override Pointer Calculate(Pointer a, Pointer b)
        {
            return a.Address64 ^ b.Address64;
        }
    }
}
