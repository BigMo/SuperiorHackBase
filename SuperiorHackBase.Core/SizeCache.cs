using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core
{
    public static class SizeCache<T> where T : struct
    {
        public static int Size { get; private set; }

        static SizeCache()
        {
            Size = Marshal.SizeOf(typeof(T));
        }
    }
}
