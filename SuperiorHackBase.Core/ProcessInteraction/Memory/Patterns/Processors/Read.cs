using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns.Processors
{
    [Processor(Pushes = 1)]
    public class Read : IPatternProcessor
    {
        public int Offset { get; private set; }
        public OperandType Type { get; private set; }

        public Read(int offset, OperandType type)
        {
            Offset = offset;
            Type = type;
        }

        public ScanResult Process(IHackContext context, ScanResult result)
        {
            Pointer operand = Pointer.Zero;
            switch(Type)
            {
                case OperandType.i8:
                    operand = (int)result.Data[Offset];
                    break;
                case OperandType.i16:
                    operand = new Pointer(BitConverter.ToInt16(result.Data, Offset));
                    break;
                case OperandType.i32:
                    operand = new Pointer(BitConverter.ToInt32(result.Data, Offset));
                    break;
                case OperandType.i64:
                    operand = new Pointer(BitConverter.ToInt64(result.Data, Offset));
                    break;
                case OperandType.u16:
                    operand = new Pointer(BitConverter.ToUInt16(result.Data, Offset));
                    break;
                case OperandType.u32:
                    operand = new Pointer(BitConverter.ToUInt32(result.Data, Offset));
                    break;
                case OperandType.u64:
                    operand = new Pointer(BitConverter.ToUInt64(result.Data, Offset));
                    break;
            }
            result.OperandStack.Push(operand);
            return result;
        }
    }
}
