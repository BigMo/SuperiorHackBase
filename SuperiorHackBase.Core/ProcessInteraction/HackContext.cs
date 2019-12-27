using System;
using System.Windows.Forms;
using SuperiorHackBase.Core.ProcessInteraction.Memory;
using SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns;
using SuperiorHackBase.Core.ProcessInteraction.Process;

namespace SuperiorHackBase.Core.ProcessInteraction
{
    public abstract class HackContext : ApplicationContext, IHackContext
    {
        public IProcess Process { get; protected set; }
        public IMemory Memory { get; protected set; }
        public float TickRate { get => timer.Tickrate; set => timer.Tickrate = value; }
        public TimeSpan Runtime => DateTime.Now - startUp;
        public event EventHandler<HackTickEventArgs> Tick;

        public class HackTickEventArgs : EventArgs
        {
            public TimeSpan Delta { get; private set; }
            public HackTickEventArgs(TimeSpan delta)
            {
                Delta = delta;
            }
        }

        private TickrateTimer timer;
        private DateTime startUp, lastRun;

        protected HackContext(IProcess process, IMemory memory)
        {
            Process = process;
            Memory = memory;
            startUp = DateTime.Now;
            timer = new TickrateTimer();
            timer.Tickrate = 60f;
            timer.Start();
            timer.Tick += Timer_Tick; ;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var run = DateTime.Now;
            var delta = lastRun - run;
            OnTick(delta);
            Tick?.Invoke(this, new HackTickEventArgs(delta));
            lastRun = run;
        }

        protected virtual void OnTick(TimeSpan delta) { }

        public ScanResult[] Scan(Pattern pattern)
        {
            return pattern.Find(this);
        }

        public virtual void Exit()
        {
            Memory.Dispose();
            this.ExitThread();
        }
    }
}
