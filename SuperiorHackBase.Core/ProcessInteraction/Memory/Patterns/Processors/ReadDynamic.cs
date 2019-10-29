using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns.Processors
{
    [Processor(Pops = 1, Pushes = 1)]
    public class ReadDynamic : IPatternProcessor
    {
        public OperandType Type { get; private set; }

        public ReadDynamic(OperandType type)
        {
            Type = type;
        }

        public void Process(IHackContext context, PatternFinding finding, Stack<Pointer> operands, ScanResult result)
        {
            Pointer address = operands.Pop();
            Pointer value = Pointer.Zero;
            switch (Type)
            {
                case OperandType.i8:
                    value = (long)context.Memory.Read<byte>(address);
                    break;
                case OperandType.i16:
                    value = (long)context.Memory.Read<short>(address);
                    break;
                case OperandType.i32:
                    value = (long)context.Memory.Read<int>(address);
                    break;
                case OperandType.i64:
                    value = (long)context.Memory.Read<long>(address);
                    break;
                case OperandType.u16:
                    value = (long)context.Memory.Read<ushort>(address);
                    break;
                case OperandType.u32:
                    value = (long)context.Memory.Read<uint>(address);
                    break;
                case OperandType.u64:
                    value = (long)context.Memory.Read<ulong>(address);
                    break;
            }
            operands.Push(value);
        }
    }
}
