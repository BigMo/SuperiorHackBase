using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns.Processors
{
    [Processor(Pops = 2, Pushes = 1)]
    public abstract class ArithmeticProcessor : IPatternProcessor
    {
        public void Process(IHackContext context, PatternFinding finding, Stack<Pointer> operands, ScanResult result)
        {
            var a = operands.Pop();
            var b = operands.Pop();
            var res = Calculate(a, b);
            operands.Push(res);
        }

        protected abstract Pointer Calculate(Pointer a, Pointer b);
    }

    
}
