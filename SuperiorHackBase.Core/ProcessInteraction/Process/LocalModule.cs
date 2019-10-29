using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Process
{
    public class LocalModule : IModule
    {
        protected ProcessModule Module { get; private set; }

        public Pointer BaseAddress => Module.BaseAddress;
        public string Name => Module.ModuleName;
        public int Size => Module.ModuleMemorySize;

        public LocalModule(ProcessModule module)
        {
            Module = module;
        }

        public override string ToString()
        {
            return $"{Name} @{BaseAddress} ({Size})";
        }
    }
}
