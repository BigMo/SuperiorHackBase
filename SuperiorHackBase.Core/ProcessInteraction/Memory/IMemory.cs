using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory
{
    public interface IMemory : IDisposable
    {
        bool Write(Pointer address, byte[] data, int offset, int count);
        bool Write(Pointer address, byte[] data);
        bool Write<T>(Pointer address, T data) where T : struct;
        bool WriteMany<T>(Pointer address, T[] data) where T : struct;

        bool WriteString(Pointer address, string text, Encoding encoding, byte[] terminator);

        bool Read(Pointer address, byte[] data, int offset, int count);
        bool Read(Pointer address, byte[] data);
        bool Read<T>(Pointer address, out T data) where T : struct;
        bool ReadMany<T>(Pointer address, ref T[] data) where T : struct;
        T Read<T>(Pointer address) where T : struct;
        T[] ReadMany<T>(Pointer address, int count) where T : struct;

        bool ReadString(Pointer address, out string text, Encoding encoding, byte[] terminator, int bufferSize, int maxByteCount);
        string ReadString(Pointer address, Encoding encoding, byte[] terminator, int bufferSize, int maxByteCount);
        bool ReadFixedString(Pointer address, out string text, Encoding encoding, int byteCount);
        string ReadFixedString(Pointer address, int length, Encoding encoding);

        Pointer ResolvePointerChain(Pointer address, params Pointer[] offsets);

        bool IsValid(Pointer address);
        long BytesRead { get; }
        long BytesWrite { get; }
    }
}
