using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns.Processors
{
    [Processor(Pops = 2,Pushes = 2)]
    public class Swap : IPatternProcessor
    {
        public void Process(IHackContext context, PatternFinding finding, Stack<Pointer> operands, ScanResult result)
        {
            var top = operands.Pop();
            var bottom = operands.Pop();
            operands.Push(top);
            operands.Push(bottom);
        }
    }
}
