using SuperiorHackBase.Core.Logic.Modules;
using SuperiorHackBase.Core.ProcessInteraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.Logic
{
    public class Hack
    {
        public IHackContext Context { get; private set; }

        public HackModule[] Modules { get; private set; }
        public ModuleRunner[] Runners { get; private set; }

        public Hack(IHackContext context, HackModule[] modules)
        {
            Context = context;
            Modules = modules;
            Runners = modules.Select(x => x.Runner).ToArray();
        }

        public void Start()
        {
            foreach (var r in Runners)
                r.Enabled = true;
        }

        public void Stop()
        {
            foreach (var r in Runners)
                r.Enabled = false;
        }

        public T GetModule<T>(bool inherits = false) where T : HackModule
        {
            var type = typeof(T);
            foreach (var m in Modules)
                if (m.GetType() ==  type || m.GetType().IsSubclassOf(type))
                    return (T)m;

            return null;
        }
        public T[] GetModules<T>(bool inherits = false) where T : HackModule
        {
            var type = typeof(T);
            return Modules.Where(x => x.GetType() == type || x.GetType().IsSubclassOf(type)).Cast<T>().ToArray();
        }
    }
}
