using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.Process
{
    public class LocalModule : IGameModule
    {
        protected ProcessModule Module { get; private set; }

        public Pointer BaseAddress => Module.BaseAddress;
        public string Name => Module.ModuleName;

        public LocalModule(ProcessModule module)
        {
            Module = module;
        }
    }
}
