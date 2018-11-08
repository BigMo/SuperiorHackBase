using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core
{
    [Serializable]
    public unsafe struct Pointer
    {
        private void* address;

        public static bool Is32Bit => IntPtr.Size == 4;
        public static bool Is64Bit => IntPtr.Size == 8;

        public ulong Address64 { get { return (ulong)address; } }
        public uint Address32 { get { return (uint)address; } }
        public IntPtr IntPtr { get { return new IntPtr(address); } }
        public static Pointer Zero => new Pointer(0);
        public static Pointer Max => new Pointer(Is64Bit ? 0x7FFFFFFFFFFFFFFF : 0x7FFFFFFF);

        public Pointer(void* address)
        {
            this.address = address;
        }
        public Pointer(int address) : this((long)address) { }
        public Pointer(long address) : this((void*)address) { }
        public Pointer(uint address) : this((ulong)address) { }
        public Pointer(ulong address) : this((void*)address) { }
        public Pointer(IntPtr address) : this(address.ToPointer()) { }


        public override string ToString()
        {
            if (Is64Bit)
                return $"0x{((ulong)address).ToString("X16")}";
            else
                return $"0x{((ulong)address).ToString("X8")}";
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is Pointer && Equals((Pointer)obj);
        }

        public override int GetHashCode()
        {
            if (Is32Bit)
                return (int)Address32;
            return Address64.GetHashCode();
        }

        #region Operators
        public static implicit operator IntPtr(Pointer ptr)
        {
            return new IntPtr(ptr.address);
        }
        public static implicit operator Pointer(IntPtr ptr)
        {
            return new Pointer(ptr);
        }
        public static implicit operator Pointer(ulong ptr)
        {
            return new Pointer(ptr);
        }
        public static implicit operator Pointer(uint ptr)
        {
            return new Pointer(ptr);
        }
        public static implicit operator Pointer(long ptr)
        {
            return new Pointer(ptr);
        }
        public static implicit operator Pointer(int ptr)
        {
            return new Pointer(ptr);
        }

        public static Pointer operator +(Pointer ptr, int offset)
        {
            return new Pointer(ptr.Address64 + (ulong)offset);
        }
        public static Pointer operator +(Pointer ptr, long offset)
        {
            return new Pointer(ptr.Address64 + (ulong)offset);
        }
        public static Pointer operator +(Pointer ptr, IntPtr offset)
        {
            return new Pointer(ptr.Address64 + (ulong)offset.ToInt64());
        }
        public static Pointer operator -(Pointer ptr, int offset)
        {
            return new Pointer(ptr.Address64 - (ulong)offset);
        }
        public static Pointer operator -(Pointer ptr, long offset)
        {
            return new Pointer(ptr.Address64 - (ulong)offset);
        }
        public static Pointer operator -(Pointer ptr, IntPtr offset)
        {
            return new Pointer(ptr.Address64 - (ulong)offset.ToInt64());
        }
        public static Pointer operator *(Pointer ptr, int offset)
        {
            return new Pointer(ptr.Address64 * (ulong)offset);
        }
        public static Pointer operator *(Pointer ptr, long offset)
        {
            return new Pointer(ptr.Address64 * (ulong)offset);
        }
        public static Pointer operator *(Pointer ptr, IntPtr offset)
        {
            return new Pointer(ptr.Address64 * (ulong)offset.ToInt64());
        }
        public static Pointer operator /(Pointer ptr, int offset)
        {
            return new Pointer(ptr.Address64 / (ulong)offset);
        }
        public static Pointer operator /(Pointer ptr, long offset)
        {
            return new Pointer(ptr.Address64 / (ulong)offset);
        }
        public static Pointer operator /(Pointer ptr, IntPtr offset)
        {
            return new Pointer(ptr.Address64 / (ulong)offset.ToInt64());
        }
        public static bool operator ==(Pointer ptrA, Pointer ptrB)
        {
            return ptrA.address == ptrB.address;
        }
        public static bool operator !=(Pointer ptrA, Pointer ptrB)
        {
            return ptrA.address != ptrB.address;
        }
        public static bool operator ==(Pointer ptrA, IntPtr ptrB)
        {
            return ptrA.Address64 == (ulong)ptrB.ToInt64();
        }
        public static bool operator !=(Pointer ptrA, IntPtr ptrB)
        {
            return ptrA.Address64 != (ulong)ptrB.ToInt64();
        }
        public static bool operator <(Pointer ptrA, Pointer ptrB)
        {
            return ptrA.address < ptrB.address;
        }
        public static bool operator >(Pointer ptrA, Pointer ptrB)
        {
            return ptrA.address > ptrB.address;
        }
        public static bool operator <=(Pointer ptrA, Pointer ptrB)
        {
            return ptrA.address <= ptrB.address;
        }
        public static bool operator >=(Pointer ptrA, Pointer ptrB)
        {
            return ptrA.address >= ptrB.address;
        }
        public static bool operator <(Pointer ptrA, IntPtr ptrB)
        {
            return ptrA.Address64 < (ulong)ptrB.ToInt64();
        }
        public static bool operator >(Pointer ptrA, IntPtr ptrB)
        {
            return ptrA.Address64 > (ulong)ptrB.ToInt64();
        }
        public static bool operator <=(Pointer ptrA, IntPtr ptrB)
        {
            return ptrA.Address64 <= (ulong)ptrB.ToInt64();
        }
        public static bool operator >=(Pointer ptrA, IntPtr ptrB)
        {
            return ptrA.Address64 >= (ulong)ptrB.ToInt64();
        }
        #endregion
    }
}
