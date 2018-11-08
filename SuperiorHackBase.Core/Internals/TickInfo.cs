using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.Internals
{
    public class TickInfo
    {
        public TimeSpan TimeSinceStart { get; private set; }
        public TimeSpan TimeDelta { get; private set; }
        public DateTime Time { get; private set; }
        public DateTime Start { get; private set; }

        public long CurrentTick { get; private set; }
        public double AverageTickRate => (double)CurrentTick / TimeSinceStart.TotalSeconds;

        internal TickInfo(DateTime start, DateTime lastTick, long tickCount)
        {
            Start = start;
            Time = DateTime.Now;
            TimeSinceStart = Time - start;
            TimeDelta = Time - lastTick;
            CurrentTick = tickCount + 1;
        }

        public TickInfo Next()
        {
            return new TickInfo(Start, Time, CurrentTick);
        }
    }
}
