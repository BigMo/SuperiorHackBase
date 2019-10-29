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

        public PushResult(string name = ScanResult.DEFAULT_VALUE_NAME)
        {
            Name = name;
        }

        public void Process(IHackContext context, PatternFinding finding, Stack<Pointer> operands, ScanResult result)
        {
            operands.Push(result.Values[Name]);
        }
    }
}
