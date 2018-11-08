using SuperiorHackBase.Core.ProcessInteraction.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns
{
    public class ScanResult
    {
        public IModule Module { get; private set; }
        public Pointer Address { get; private set; }
        public byte[] Data { get; private set; }
        public Stack<Pointer> OperandStack { get; private set; }
        public Dictionary<string,Pointer> Results { get; private set; }
        public Pointer Result { get { return Results.ContainsKey("Result") ? Results["Result"] : Pointer.Zero; } }

        public ScanResult(IModule module, Pointer address, byte[] data) : this (module, address, data, new Stack<Pointer>(), new Dictionary<string, Pointer>())
        {
        }

        public ScanResult(IModule module, Pointer address, byte[] data, Stack<Pointer> operandStack, Dictionary<string, Pointer> results)
        {
            Module = module;
            Address = address;
            Data = data;
            OperandStack = operandStack;
            Results = results;
            if (!results.ContainsKey("Result"))
                results["Result"] = Pointer.Zero;
        }
    }
}
