using System.Collections.Generic;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns
{
    public interface IPatternProcessor
    {
        void Process(IHackContext context, PatternFinding finding, Stack<Pointer> operands, ScanResult result);
    }
}