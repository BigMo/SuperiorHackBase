using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns
{
    public class PatternFinding
    {
        public byte[] Data { get; private set; }
        public Pointer Address { get; private set; }
        
        public PatternFinding(byte[] data, Pointer address)
        {
            Data = data;
            Address = address;
        }

        public ScanResult Process(IHackContext ctx, IPatternProcessor[] processors)
        {
            var result = new ScanResult();
            var operandStack = new Stack<Pointer>();
            
            foreach (var processor in processors)
                processor.Process(ctx, this, operandStack, result);

            if (operandStack.Count == 1)
                result.Value = operandStack.Pop();

            return result;
        }
    }
}
