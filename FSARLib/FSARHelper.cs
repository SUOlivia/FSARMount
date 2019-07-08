using System;
using System.Runtime.CompilerServices;

namespace FSARLib
{
    public static class FSARHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void fastCopyBlock(Byte[] src, int src_index, Byte[] dest, int dest_index, int lenght)
        {
            void* from = Unsafe.AsPointer(ref src[src_index]);
            void* to = Unsafe.AsPointer(ref dest[dest_index]);

            Unsafe.CopyBlockUnaligned(to, from, (uint) lenght);
        } 
    }
}