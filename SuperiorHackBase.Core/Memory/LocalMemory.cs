using SuperiorHackBase.Core.Process;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.Memory
{
    public class LocalMemory : IMemory, IDisposable
    {
        public TimeSpan PageCacheDuration { get; set; }
        public IntPtr MemoryHandle { get; private set; }

        public long BytesRead { get; private set; }
        public long BytesWrite { get; private set; }

        private WinAPI.MEMORY_BASIC_INFORMATION[] pageCache;
        private DateTime pageCacheTime;
        private LocalProcess process;
        private bool raiseExceptions;

        internal LocalMemory(LocalProcess process, bool raiseExceptions, WinAPI.ProcessAccessFlags flags)
        {
            this.process = process;
            this.raiseExceptions = raiseExceptions;
            this.pageCacheTime = DateTime.MinValue;
            PageCacheDuration = TimeSpan.FromSeconds(5);

            if ((flags & WinAPI.ProcessAccessFlags.VirtualMemoryRead) == 0)
                throw new ArgumentException("Flags require at least ProcessAccessFlags.VirtualMemoryRead to be set");

            MemoryHandle = WinAPI.OpenProcess(flags, false, process.PID);
            if (MemoryHandle == IntPtr.Zero)
                throw new Exception("Failed to aquire memory handle", new Win32Exception(Marshal.GetLastWin32Error()));
        }

        #region Read
        public bool Read(Pointer address, byte[] data, int offset, int count)
        {
            IntPtr readBytes = IntPtr.Zero;
            var res = WinAPI.ReadProcessMemory(MemoryHandle, address, data, count, out readBytes);
            BytesRead += readBytes.ToInt64();
            if (!res || readBytes.ToInt32() != count)
                if (raiseExceptions)
                    throw new ProcessMemoryException(address, readBytes.ToInt32(), count, Marshal.GetLastWin32Error(), res);
                else
                    return false;

            return true;
        }

        public bool Read(Pointer address, byte[] data)
        {
            return Read(address, data, 0, data.Length);
        }

        public bool Read<T>(Pointer address, out T data) where T : struct
        {
            var buffer = new byte[SizeCache<T>.Size];
            Read(address, buffer);
            data = BytesToT<T>(buffer);
            return true;
        }

        public bool ReadMany<T>(Pointer address, ref T[] data) where T : struct
        {
            var buffer = new byte[SizeCache<T>.Size * data.Length];
            Read(address, buffer);
            BytesToTs<T>(buffer, ref data);
            return true;
        }

        public T Read<T>(Pointer address) where T : struct
        {
            var data = default(T);
            Read<T>(address, out data);
            return data;
        }

        public T[] ReadMany<T>(Pointer address, int count) where T : struct
        {
            var data = new T[count];
            ReadMany<T>(address, ref data);
            return data;
        }
        #endregion

        #region Write
        public bool Write(Pointer address, byte[] data, int offset, int count)
        {
            IntPtr writeBytes = IntPtr.Zero;
            var res = WinAPI.WriteProcessMemory(MemoryHandle, address, data, count, out writeBytes);
            BytesWrite += writeBytes.ToInt64();
            if (!res || writeBytes.ToInt32() != count)
                if (raiseExceptions)
                    throw new ProcessMemoryException(address, writeBytes.ToInt32(), count, Marshal.GetLastWin32Error(), res);
                else
                    return false;

            return true;
        }

        public bool Write(Pointer address, byte[] data)
        {
            return Write(address, data, 0, data.Length);
        }

        public bool Write<T>(Pointer address, T data) where T : struct
        {
            var buffer = TToBytes<T>(data);
            return Write(address, buffer);
        }

        public bool WriteMany<T>(Pointer address, T[] data) where T : struct
        {
            var buffer = TsToBytes<T>(data);
            return Write(address, buffer);
        }
        #endregion
        

        public bool WriteString(Pointer address, string text, Encoding encoding, byte[] terminator)
        {
            using (var mem = new MemoryStream())
            {
                var str = encoding.GetBytes(text);
                mem.Write(str, 0, str.Length);
                if (terminator != null)
                    mem.Write(terminator, 0, terminator.Length);

                return Write(address, mem.ToArray());
            }
        }

        public bool ReadString(Pointer address, out string text, Encoding encoding, byte[] terminator, int bufferSize, int maxByteCount)
        {
            var buffer = new byte[bufferSize];
            var _address = address;
            var count = 0;
            using (var mem = new MemoryStream())
            {
                Read(_address, buffer);
                var idx = Find(buffer, terminator);
                if (idx > 0)
                    count += idx;
                if (count > maxByteCount)
                {
                    text = encoding.GetString(mem.ToArray());
                    return true;
                }

                if (idx == -1)
                {
                    mem.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    mem.Write(buffer, 0, idx);
                    text = encoding.GetString(mem.ToArray());
                    return true;
                }
                _address += bufferSize;
            }
            text = null;
            return false;
        }

        private int Find(byte[] haystack, byte[] needle)
        {
            for (int i = 0; i < haystack.Length - needle.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < needle.Length; j++)
                {
                    if (haystack[i + j] != needle[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    return i;
            }

            return -1;
        }

        public bool ReadFixedString(Pointer address, out string text, Encoding encoding, int byteCount)
        {
            var buffer = new byte[byteCount];
            Read(address, buffer);
            text = encoding.GetString(buffer);
            return true;
        }

        public void Dispose()
        {
            if (MemoryHandle != IntPtr.Zero)
            {
                if (!WinAPI.CloseHandle(MemoryHandle) && raiseExceptions)
                    throw new Exception("Failed to close memory handle to process", new Win32Exception(Marshal.GetLastWin32Error()));
                MemoryHandle = Pointer.Zero;
            }
        }

        #region MARSHALLING
        private unsafe static byte[] TToBytes<T>(T value) where T : struct
        {
            byte[] data = new byte[SizeCache<T>.Size];

            fixed (byte* b = data)
                Marshal.StructureToPtr(value, (IntPtr)b, true);

            return data;
        }

        private unsafe static byte[] TsToBytes<T>(T[] values) where T : struct
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

        private static unsafe T BytesToT<T>(byte[] data, T defVal = default(T)) where T : struct
        {
            T structure = defVal;

            fixed (byte* b = data)
                structure = (T)Marshal.PtrToStructure((IntPtr)b, typeof(T));

            return structure;
        }
        private static unsafe void BytesToTs<T>(byte[] data, ref T[] array) where T : struct
        {
            fixed (byte* b = data)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = (T)Marshal.PtrToStructure((IntPtr)(b + SizeCache<T>.Size * i), typeof(T));
                }
            }
        }
        #endregion

        public bool IsValid(Pointer address)
        {
            return process.Pages.Any(x => address >= x.BaseAddress && address <= x.BaseAddress + (Pointer)x.RegionSize);
        }
    }
}
