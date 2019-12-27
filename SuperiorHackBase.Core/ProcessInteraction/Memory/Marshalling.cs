using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory
{
    public static class Marshalling
    {
        public unsafe static byte[] TToBytes<T>(T value) where T : struct
        {
            byte[] data = new byte[SizeCache<T>.Size];

            fixed (byte* b = data)
                Marshal.StructureToPtr(value, (IntPtr)b, true);

            return data;
        }

        public unsafe static byte[] TsToBytes<T>(T[] values) where T : struct
        {
            byte[] data = new byte[SizeCache<T>.Size * values.Length];

            fixed (byte* b = data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    Marshal.StructureToPtr(values[i], (IntPtr)(b + SizeCache<T>.Size * i), true);
                }
            }
            return data;
        }

        public static unsafe T BytesToT<T>(byte[] data, T defVal = default(T)) where T : struct
        {
            T structure = defVal;

            fixed (byte* b = data)
                structure = (T)Marshal.PtrToStructure((IntPtr)b, typeof(T));

            return structure;
        }
        public static unsafe T BytesToTOffset<T>(byte[] data, int offset, T defVal = default(T)) where T : struct
        {
            T structure = defVal;

            fixed (byte* b = data)
                structure = (T)Marshal.PtrToStructure((IntPtr)(b + offset), typeof(T));

            return structure;
        }
        public static unsafe void BytesToTs<T>(byte[] data, ref T[] array) where T : struct
        {
            fixed (byte* b = data)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = (T)Marshal.PtrToStructure((IntPtr)(b + SizeCache<T>.Size * i), typeof(T));
                }
            }
        }
    }
}
