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
        public ScanResult Process(IHackContext context, ScanResult result)
        {
            var stack = result.OperandStack;

            var a = stack.Pop();
            var b = stack.Pop();
            var res = Calculate(a, b);
            stack.Push(res);

            return result;
        }

        protected abstract Pointer Calculate(Pointer a, Pointer b);
    }

    
}
