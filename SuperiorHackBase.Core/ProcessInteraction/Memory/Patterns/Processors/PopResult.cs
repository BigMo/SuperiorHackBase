using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns.Processors
{
    [Processor(Pops = 1)]
    public class PopResult : IPatternProcessor
    {
        public string Name { get; private set; }

        public PopResult(string name)
        {
            Name = name;
        }
        
        public void Process(IHackContext context, PatternFinding finding, Stack<Pointer> operands, ScanResult result)
        {
            result.Values[Name] = operands.Pop();
        }
    }
}
