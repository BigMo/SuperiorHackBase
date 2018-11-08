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

        public ScanResult Process(IHackContext context, ScanResult result)
        {
            result.Results[Name] = result.OperandStack.Pop();
            return result;
        }
    }
}
