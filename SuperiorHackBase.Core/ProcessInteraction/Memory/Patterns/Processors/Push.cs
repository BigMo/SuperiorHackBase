using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns.Processors
{
    [Processor(Pushes = 1)]
    public class Push : IPatternProcessor
    {
        public Pointer Value { get; private set; }

        public Push(Pointer value)
        {
            Value = value;
        }

        public ScanResult Process(IHackContext context, ScanResult result)
        {
            result.OperandStack.Push(Value);
            return result;
        }
    }
}
