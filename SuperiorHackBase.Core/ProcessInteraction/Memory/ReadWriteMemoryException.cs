using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory
{
    public class ReadWriteMemoryException : Exception
    {
        public Pointer Address { get; private set; }
        public int Processed { get; private set; }
        public int Count { get; private set; }
        public int ErrorCode { get; private set; }
        public bool Result { get; private set; }

        private static string CreateMessage(Pointer address, int processed, int count, int errorCode, bool result)
        {
            return string.Format("Failed to read/write data: result {0}, errorCode {1}, processed {2}/{3} bytes at {4}",
                result,
                errorCode,
                processed,
                count,
                address);
        }

        public ReadWriteMemoryException(Pointer address, int processed, int count, int errorCode, bool result) : base(CreateMessage(address, processed, count, errorCode, result), new Win32Exception(errorCode))
        {
            Address = address;
            Processed = processed;
            Count = count;
            ErrorCode = errorCode;
            Result = result;
        }
    }
}
