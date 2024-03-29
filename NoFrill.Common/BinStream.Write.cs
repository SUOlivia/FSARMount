using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace NoFrill.Common
{
    public static partial class BinStream
    {
        [ThreadStatic] static byte[] scratchData;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] EnsureScratch()
        {
            if (scratchData == null)
            {
                scratchData = new byte[8];
            }

            return scratchData;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt64BE(this Stream data, UInt64 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt64BE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt64LE(this Stream data, UInt64 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt64LE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt32BE(this Stream data, UInt32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt32BE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt32LE(this Stream data, UInt32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt32LE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt16BE(this Stream data, UInt16 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt16BE(0, value);
            data.Write(scratch, 0, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt16LE(this Stream data, UInt16 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt16LE(0, value);
            data.Write(scratch, 0, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Writebyte(this Stream data, byte value)
        {
            Debug.Assert(data.CanWrite);
            data.WriteByte(value);
        }
    }
}