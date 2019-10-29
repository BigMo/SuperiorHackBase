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

        public void Process(IHackContext context, PatternFinding finding, Stack<Pointer> operands, ScanResult result)
        {
            operands.Push(Value);
        }
    }
}
