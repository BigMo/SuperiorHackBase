using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperiorHackBase.Core.Process;

namespace SuperiorHackBase.Core.Memory
{
    public class CachedStreamMemory : StreamMemory
    {
        private WinAPI.MEMORY_BASIC_INFORMATION pageInfo;
        private byte[] pageData;

        public CachedStreamMemory(IMemory mem, IGameProcess proc) : base(mem, proc)
        {
            pageInfo = new WinAPI.MEMORY_BASIC_INFORMATION() { RegionSize = IntPtr.Zero };
            pageData = null;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!pageInfo.Contains(Position)) //Try to cache page
            {
                //Update pages and recheck before throwing
                process.UpdatePages();
                if (!IsInPages(Position))
                    throw new Exception();

                //Dump page
                pageInfo = process.Pages.First(_page => _page.Contains(Position));
                var _oldPos = Position;
                Position = pageInfo.BaseAddress.ToInt64();
                pageData = new byte[pageInfo.RegionSize.ToInt64()];
                base.Read(pageData, 0, pageData.Length); //Read whole page
                Position = _oldPos; //Restore former position
            }
            var pageOffset = Position - pageInfo.BaseAddress.ToInt64();
            var _count = (int)Math.Min(count, pageInfo.RegionSize.ToInt64() - pageOffset);
            Array.Copy(pageData, pageOffset, buffer, offset, _count);
            Position += _count;
            return _count;
        }
    }
}
