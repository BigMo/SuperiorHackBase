using SuperiorHackBase.Core.Logic.Modules;
using SuperiorHackBase.Core.ProcessInteraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.Logic.Modules
{
    public class ModuleRunner
    {
        private Task task;
        private bool enabled;
        public HackModule Module { get; private set; }
        public HackModuleAttribute Attribute { get; private set; }

        public bool Running { get { return task.Status == TaskStatus.Running; } }
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    if (enabled && !Running && task.Status != TaskStatus.WaitingToRun)
                    {
                        task.Start();
                    }
                    else if (!enabled)
                    {
                        task.Wait();
                    }
                }
            }
        }


        internal ModuleRunner(HackModule module)
        {
            Module = module;

            var attributes = module.GetType().GetCustomAttributes(typeof(HackModuleAttribute), false);
            if (attributes == null || attributes.Length == 0)
                throw new Exception("HackModule requires a HackModuleAttribute");
            Attribute = attributes.Cast<HackModuleAttribute>().OrderBy(x => x.TicksPerSecond).Last();

            task = new Task(async () => await RunModule(Attribute.TicksPerSecond));
        }

        private async Task RunModule(int rate)
        {
            var cycleMs = Math.Max(1, (int)Math.Floor(1000f / rate));

            DateTime start, end;
            TimeSpan completeCycle = TimeSpan.FromMilliseconds(cycleMs);
            TimeSpan dutyCycle, idleCycle;

            while (enabled)
            {
                start = DateTime.Now;
                Module.Tick();
                end = DateTime.Now;

                dutyCycle = end - start;
                idleCycle = completeCycle - dutyCycle;
                if (idleCycle.Ticks < 0)
                {
                    Console.WriteLine("{0} lagging behind: took {1}ms of {2}ms planned", Module.GetType().Name, dutyCycle.TotalMilliseconds, completeCycle.TotalMilliseconds);
                }
                else if ((int)idleCycle.TotalMilliseconds > 0)
                {
                    Console.WriteLine("{0} idling: took {1}ms of {2}ms planned, idling {3}ms", Module.GetType().Name, dutyCycle.TotalMilliseconds, completeCycle.TotalMilliseconds, idleCycle.TotalMilliseconds);
                    await Task.Delay(idleCycle);
                }
            }
        }
    }
}
