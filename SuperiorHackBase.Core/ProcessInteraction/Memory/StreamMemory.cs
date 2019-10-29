using SuperiorHackBase.Core.ProcessInteraction.Process;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory
{
    public class StreamMemory : Stream
    {
        private long position;
        protected IMemory memory;
        protected IProcess process;

        public override bool CanRead { get; } = true;
        public override bool CanSeek { get; } = true;
        public override bool CanWrite { get; } = true;
        public override long Length { get; } = Pointer.Is32Bit ? int.MaxValue : long.MaxValue;

        public override long Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    if (!IsInPages(value))
                    {
                        process.UpdatePages(); //Update pages once and re-check before throwing
                        if (!IsInPages(value))
                            throw new Exception("Address outside of paged memory");
                    }
                    position = value;
                }
            }
        }

        public StreamMemory(IMemory mem, IProcess proc)
        {
            memory = mem;
            process = proc;
        }

        protected bool IsInPages(Pointer address)
        {
            return process.Pages.Any(p => p.Contains(address, 0));
        }
        public override void Flush() { throw new NotImplementedException(); }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (memory.Read(new Pointer((ulong)position), buffer, offset, count))
            {
                position += buffer.Length;
                return buffer.Length;
            }
            return 0;
        }
        
        public override void Write(byte[] buffer, int offset, int count)
        {
            memory.Write(new Pointer((ulong)position), buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.End:
                    Position = Length + offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
            }
            return position;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
    }
}
