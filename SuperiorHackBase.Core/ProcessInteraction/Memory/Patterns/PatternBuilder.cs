using SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns
{
    public class PatternBuilder
    {
        private Pointer scanStart, scanEnd;
        private string module;
        private byte[] pattern;
        private string mask;
        private List<IPatternProcessor> processors;
        private bool findMultiple, relative;

        private PatternBuilder(byte[] pattern, string mask)
        {
            this.module = null;
            this.scanEnd = Pointer.Max;
            this.scanStart = Pointer.Zero;
            this.pattern = pattern;
            this.mask = mask;
            processors = new List<IPatternProcessor>();
        }

        public static PatternBuilder FromHexString(string pattern, string mask = null)
        {
            var bytes = pattern.Select(x => (byte)x).ToArray();
            if (mask == null)
                mask = string.Join("", pattern.Select(x => x == '\x0' ? "?" : "x").ToArray());
            return new PatternBuilder(bytes, mask);
        }
        public static PatternBuilder FromString(string pattern, string delimiter = " ", string wildcard = "?")
        {
            var parts = pattern.Split(delimiter.ToArray());
            var bytes = parts.Select(x => x == wildcard ? (byte)0 : byte.Parse(x, System.Globalization.NumberStyles.HexNumber)).ToArray();
            var mask = string.Join("", parts.Select(x => x == wildcard ? "?" : "x").ToArray());
            return new PatternBuilder(bytes, mask);
        }
        public static PatternBuilder FromArray(byte[] pattern, byte wildcard = 0x00)
        {
            var mask = string.Join("", pattern.Select(x => x == wildcard ? "?`" : "x").ToArray());
            return new PatternBuilder(pattern, mask);
        }

        public PatternBuilder Swap()
        {
            this.processors.Add(new Swap());
            return this;
        }
        public PatternBuilder SetModule(string module, bool relative = false)
        {
            this.module = module;
            this.relative = true;
            return this;
        }
        public PatternBuilder PushModuleBase(string module)
        {
            processors.Add(new PushBaseAddress(module));
            return this;
        }
        public PatternBuilder SetScanRange(Pointer scanStart, Pointer scanEnd)
        {
            this.scanStart = scanStart;
            this.scanEnd = scanEnd;
            return this;
        }
        public PatternBuilder Multiple()
        {
            findMultiple = true;
            return this;
        }
        public PatternBuilder Single()
        {
            findMultiple = false;
            return this;
        }

        public PatternBuilder Add()
        {
            processors.Add(new Add());
            return this;
        }
        public PatternBuilder Subtract()
        {
            processors.Add(new Substract());
            return this;
        }
        public PatternBuilder Multiply()
        {
            processors.Add(new Multiply());
            return this;
        }
        public PatternBuilder Divide()
        {
            processors.Add(new Divide());
            return this;
        }
        public PatternBuilder And()
        {
            processors.Add(new And());
            return this;
        }
        public PatternBuilder Or()
        {
            processors.Add(new Or());
            return this;
        }
        public PatternBuilder Xor()
        {
            processors.Add(new Xor());
            return this;
        }
        public PatternBuilder PopResult(string name = "Result")
        {
            processors.Add(new PopResult(name));
            return this;
        }
        public PatternBuilder Push(Pointer value)
        {
            processors.Add(new Push(value));
            return this;
        }
        public PatternBuilder PushResult(string name)
        {
            processors.Add(new PushResult(name));
            return this;
        }
        public PatternBuilder ReadLocal(int offset, OperandType type)
        {
            processors.Add(new ReadLocal(offset, type));
            return this;
        }
        public PatternBuilder ReadLocalDynamic(OperandType type)
        {
            processors.Add(new ReadLocalDynamic(type));
            return this;
        }
        public PatternBuilder ReadDynamic(OperandType type)
        {
            processors.Add(new ReadDynamic(type));
            return this;
        }

        public Pattern Build()
        {
            if (relative)
            {
                processors.Add(new PushBaseAddress(this.module));
                processors.Add(new Swap());
                processors.Add(new Substract());
            }
            //Analyze processors
            int totalPush = 0, totalPop = 0;
            foreach (var processor in processors)
            {
                var attr = (ProcessorAttribute)processor.GetType().GetCustomAttributes(typeof(ProcessorAttribute), true).FirstOrDefault();
                if (attr == null)
                    throw new Exception($"Missing ProcessorAttribute on {processor.GetType().Name}");
                totalPush += attr.Pushes;
                totalPop += attr.Pops;

                if (totalPush - totalPop < 0)
                    throw new Exception($"Negative stack-size processing {processor.GetType().Name}");
            }

            var sum = totalPush - totalPop;
            if (sum == 1)
                processors.Add(new PopResult(ScanResult.DEFAULT_VALUE_NAME));
            else if (sum > 1)
                throw new Exception($"There will be {sum} values left on the stack after execution");

            if (string.IsNullOrEmpty(module))
                return new Pattern(scanStart, scanEnd, pattern, mask, processors.ToArray(), findMultiple);
            else
                return new Pattern(module, pattern, mask, processors.ToArray(), findMultiple);
        }
    }
}
