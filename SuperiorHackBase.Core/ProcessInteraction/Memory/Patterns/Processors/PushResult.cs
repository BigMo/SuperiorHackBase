using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns.Processors
{
    [Processor(Pushes = 1)]
    public class PushResult : IPatternProcessor
    {
        public string Name { get; private set; }

        public PushResult(string name = "Result")
        {
            Name = name;
        }

        public ScanResult Process(IHackContext context, ScanResult result)
        {
            result.OperandStack.Push(result.Results[Name]);
            return result;
        }
    }
}
