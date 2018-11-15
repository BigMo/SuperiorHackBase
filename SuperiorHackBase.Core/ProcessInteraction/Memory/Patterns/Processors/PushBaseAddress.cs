using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns.Processors
{
    [Processor(Pushes = 1)]
    public class PushBaseAddress : IPatternProcessor
    {
        public string ModuleName { get; private set; }

        public PushBaseAddress(string moduleName)
        {
            ModuleName = moduleName;
        }

        public void Process(IHackContext context, PatternFinding finding, Stack<Pointer> operands, ScanResult result)
        {
            operands.Push(context.Process.Modules.FirstOrDefault(x => x.Name == ModuleName).BaseAddress);
        }
    }
}
