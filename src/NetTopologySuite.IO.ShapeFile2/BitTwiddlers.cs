using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetTopologySuite.IO
{
    internal static class BitTwiddlers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int ReverseEndianness(int val) => BinaryPrimitives.ReverseEndianness(val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint WordsToBytes(int value) => unchecked((uint)(value + (long)value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int BytesToWords(uint value)
        {
            if ((value & 1) == 1)
            {
                ThrowArgumentExceptionForOddNumber();
            }

            return checked((int)(value / 2));
        }

        internal static void EnsureLittleEndian()
        {
            if (!BitConverter.IsLittleEndian)
            {
                ThrowNotSupportedExceptionForBigEndian();
            }
        }

        internal static unsafe bool Equals<T>(ref T first, ref T second)
            where T : unmanaged
        {
            fixed (T* pFirst = &first, pSecond = &second)
            {
                var span1 = new ReadOnlySpan<byte>(pFirst, sizeof(T));
                var span2 = new ReadOnlySpan<byte>(pSecond, sizeof(T));
                return span1.SequenceEqual(span2);
            }
        }

        internal static unsafe int GetHashCode<T>(ref T val)
            where T : unmanaged
        {
            fixed (T* pVal = &val)
            {
                // TODO: use a binary hash function
                var thisSpan = new ReadOnlySpan<byte>(pVal, sizeof(T));
                var thisIntSpan = MemoryMarshal.Cast<byte, int>(thisSpan);

                int hc = 17;
                foreach (int x in thisIntSpan)
                {
                    hc = unchecked((hc * 31) + x);
                }

                return hc;
            }
        }

#pragma warning disable CA2208 // Instantiate argument exceptions correctly
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentExceptionForOddNumber() => throw new ArgumentException("Only even numbers are supported.", "value");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowNotSupportedExceptionForBigEndian() => throw new NotSupportedException("Big-endian machines are not currently supported.");
    }
}
