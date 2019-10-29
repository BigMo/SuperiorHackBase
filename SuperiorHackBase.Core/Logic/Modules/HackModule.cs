using SuperiorHackBase.Core.Internals;
using SuperiorHackBase.Core.ProcessInteraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.Logic.Modules
{
    public abstract class HackModule
    {
        private TickInfo tick;
        protected IHackContext Context { get; private set; }

        public ModuleRunner Runner { get; private set; }

        protected HackModule(IHackContext context)
        {
            this.Context = context;
            tick = new TickInfo(DateTime.Now, DateTime.Now, 0);
            Runner = new ModuleRunner(this);
        }

        public void Tick()
        {
            if (tick.CurrentTick == 0)
                OnFirstTick();

            tick = tick.Next();

            OnBeforeTick(tick);
            OnTick(tick);
            OnAfterTick(tick);
        }

        protected virtual void OnFirstTick() { }
        protected virtual void OnBeforeTick(TickInfo tick) { }
        protected virtual void OnAfterTick(TickInfo tick) { }

        protected abstract void OnTick(TickInfo tick);
    }
}
