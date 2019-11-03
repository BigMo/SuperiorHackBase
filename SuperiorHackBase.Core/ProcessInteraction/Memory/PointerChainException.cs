using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory
{
    public class PointerChainException : Exception
    {
        public Pointer BaseAddress { get; private set; }
        public Pointer[] Offsets { get; private set; }
        public int OffsetIndex { get; private set; }

        private static string CreateMessage(Pointer baseAddress, Pointer[] offsets, int offsetIndex)
        {
            return string.Format("Failed to resolve pointer-chain {0} ] {1}] at offset-index {2}",
                baseAddress,
                string.Join("] ", offsets.Select(x=>x).ToArray()),
                offsetIndex);
        }

        public PointerChainException(Pointer baseAddress, Pointer[] offsets, int offsetIndex, Exception innerException = null) : base(CreateMessage(baseAddress, offsets, offsetIndex), innerException)
        {
            BaseAddress = baseAddress;
            Offsets = offsets;
            OffsetIndex = offsetIndex;
        }
    }
}
